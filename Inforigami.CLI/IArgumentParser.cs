namespace Inforigami.CLI
{
    using System.Collections.Generic;

    public interface IArgumentParser
    {
        IEnumerable<Argument> Parse(string argumentText);
    }
}