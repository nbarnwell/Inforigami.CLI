namespace Inforigami.CLI
{
    public interface ICommandBuilder
    {
        object Create(Instruction instruction);
    }
}