namespace Inforigami.CLI
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Inforigami.Regalo.Interfaces;

    public class DirectoryScanningCommandTypeProvider : ICommandTypeProvider
    {
        public IEnumerable<Type> GetCommandTypes()
        {
            return Directory.GetFiles(Environment.CurrentDirectory)
                            .Where(IsAssembly)
                            .Select(Assembly.LoadFrom)
                            .SelectMany(x => x.GetTypes())
                            .Where(IsDefinedAsCommand);
        }

        private bool IsDefinedAsCommand(Type type)
        {
            return typeof(ICommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract;
        }

        private bool IsAssembly(string arg)
        {
            return new[] { ".exe", ".dll" }.Contains(Path.GetExtension(arg));
        }
    }
}