namespace Inforigami.CLI
{
    public interface IInstructionParser
    {
        Instruction Parse(string instructionText);
    }
}