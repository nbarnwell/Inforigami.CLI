namespace Inforigami.CLI
{
    using System.Collections.Generic;

    public class QuotedStringTokenizer : IStringTokenizer
    {
        public IEnumerable<string> Parse(string argumentText)
        {
            var result = new List<string>();

            bool quotesActive = false;
            string current = null;
            for (int i = 0; i < argumentText.Length; i++)
            {
                var c = argumentText[i];

                if (c == '"')
                {
                    quotesActive = !quotesActive;
                    continue;
                }

                if (c == ' ')
                {
                    if (quotesActive)
                    {
                        current += c;
                    }
                    else
                    {
                        result.Add(current);
                        current = null;
                    }
                }
                else
                {
                    current += c;
                }
            }

            if (current != null)
            {
                result.Add(current);
            }

            return result;
        }
    }
}