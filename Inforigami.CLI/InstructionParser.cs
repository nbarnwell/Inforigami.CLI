namespace Inforigami.CLI
{
    using System;

    public class InstructionParser : IInstructionParser
    {
        private readonly IArgumentParser _argumentParser;

        public InstructionParser(IArgumentParser argumentParser)
        {
            if (argumentParser == null) throw new ArgumentNullException(nameof(argumentParser));
            _argumentParser = argumentParser;
        }

        public Instruction Parse(string instructionText)
        {
            var indexOfSpace = instructionText.IndexOf(' ');
            var commandName = instructionText.Substring(0, indexOfSpace >= 0 ? indexOfSpace : instructionText.Length).Trim();

            if (indexOfSpace >= 0)
            {
                var argumentText = instructionText.Substring(indexOfSpace).Trim();
                var arguments = _argumentParser.Parse(argumentText);
                
                return new Instruction(commandName, arguments);
            }

            return new Instruction(commandName);
        }
    }
}