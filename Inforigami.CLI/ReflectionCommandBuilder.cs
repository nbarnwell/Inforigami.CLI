namespace Inforigami.CLI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class ReflectionCommandBuilder : ICommandBuilder
    {
        private readonly IDictionary<string, Type> _commandTypes;

        public ReflectionCommandBuilder(ICommandTypeProvider commandTypeProvider)
        {
            if (commandTypeProvider == null) throw new ArgumentNullException(nameof(commandTypeProvider));

            _commandTypes = commandTypeProvider.GetCommandTypes()
                                               .ToDictionary(CommandNameFromType, x => x);
        }

        private string CommandNameFromType(Type commandType)
        {
            var commandName = Regex.Replace(commandType.Name, @"Command$", "");
            commandName = commandName.ToLower();
            return commandName;
        }

        public object Create(Instruction instruction)
        {
            Type commandType;
            if (!_commandTypes.TryGetValue(instruction.CommandName, out commandType))
            {
                throw new InvalidOperationException($"No command matching \"{instruction.CommandName}\" found.");
            }

            var command = Activator.CreateInstance(commandType, null);
            PopulateCommandProperties(command, instruction);

            return command;
        }

        private void PopulateCommandProperties(object command, Instruction instruction)
        {
            if (instruction.Arguments == null || instruction.Arguments.Any() == false)
            {
                return;
            }

            var properties =
                command.GetType()
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var namedArgs = instruction.Arguments.Where(x => !string.IsNullOrEmpty(x.Key));
            var unnamedArgs = instruction.Arguments.Where(x => string.IsNullOrEmpty(x.Key)).ToArray();

            IDictionary<string, PropertyInfo> remainingPropertiesMap =
                properties.ToDictionary(x => x.Name, x => x);

            foreach (var arg in namedArgs)
            {
                var property =
                    properties.SingleOrDefault(x => x.Name.Equals(arg.Key, StringComparison.CurrentCultureIgnoreCase));

                if (property == null)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Command \"{0}\" has no named argument \"{1}\"",
                            command.GetType().Name,
                            arg.Key));
                }

                property.SetValue(command, Convert.ChangeType(arg.Value, property.PropertyType));
                remainingPropertiesMap.Remove(property.Name);
            }

            var remainingProperties = properties.Where(x => remainingPropertiesMap.ContainsKey(x.Name))
                                                .ToArray();

            for (int i = 0; i < remainingProperties.Length; i++)
            {
                if (i > unnamedArgs.GetUpperBound(0))
                {
                    break;
                }

                var property = remainingProperties[i];
                var arg = unnamedArgs[i];
                property.SetValue(command, Convert.ChangeType(arg.Value, property.PropertyType));
            }
        }
    }
}