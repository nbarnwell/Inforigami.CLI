namespace Inforigami.CLI
{
    using System;

    public class UnknownCommandException : Exception
    {
        public string CommandName { get; }

        public UnknownCommandException(string message, string commandName)
            : base(message)
        {
            CommandName = commandName;
        }
    }
}