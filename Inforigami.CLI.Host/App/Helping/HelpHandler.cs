namespace Inforigami.CLI.Host.App.Helping
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Inforigami.Regalo.Messaging;

    public class HelpHandler : ICommandHandler<Help>
    {
        private readonly ICommandTypeProvider _commandTypeProvider;

        public HelpHandler(ICommandTypeProvider commandTypeProvider)
        {
            if (commandTypeProvider == null) throw new ArgumentNullException(nameof(commandTypeProvider));
            _commandTypeProvider = commandTypeProvider;
        }

        public void Handle(Help command)
        {
            Console.WriteLine("Available commands:");

            var commandNames =
                _commandTypeProvider.GetCommandTypes()
                                    .Select(x => Regex.Replace(x.Name, @"Command$", ""))
                                    .OrderBy(x => x);

            foreach (var commandName in commandNames)
            {
                Console.WriteLine(" - {0}", commandName);
            }
        }
    }
}