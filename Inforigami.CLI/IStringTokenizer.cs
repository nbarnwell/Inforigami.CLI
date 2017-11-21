namespace Inforigami.CLI
{
    using System.Collections.Generic;

    public interface IStringTokenizer
    {
        IEnumerable<string> Parse(string argumentText);
    }
}