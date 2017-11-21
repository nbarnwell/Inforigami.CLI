namespace Inforigami.CLI
{
    using System.Collections.Generic;

    public class Instruction
    {
        public string CommandName { get; }
        public IEnumerable<Argument> Arguments { get; }

        public Instruction(string commandName, IEnumerable<Argument> arguments)
        {
            CommandName = commandName;
            Arguments = arguments;
        }

        public Instruction(string commandName)
            : this(commandName, new Argument[0])
        {
        }
    }
}