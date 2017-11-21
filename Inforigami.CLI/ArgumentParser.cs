namespace Inforigami.CLI
{
    using System;
    using System.Collections.Generic;

    public class ArgumentParser : IArgumentParser
    {
        private readonly IStringTokenizer _tokenizer;

        public ArgumentParser(IStringTokenizer tokenizer)
        {
            if (tokenizer == null) throw new ArgumentNullException(nameof(tokenizer));
            _tokenizer = tokenizer;
        }

        public IEnumerable<Argument> Parse(string argumentText)
        {
            var tokens = _tokenizer.Parse(argumentText)
                                   .ToQueue();

            string key = null;
            while (tokens.Count > 0)
            {
                var item = tokens.Dequeue();

                if (item.StartsWith("-"))
                {
                    if (key != null)
                    {
                        yield return new Argument(key, null);
                    }

                    key = item.Trim('-');
                }
                else
                {
                    if (key != null)
                    {
                        yield return new Argument(key, item);
                        key = null;
                    }
                    else
                    {
                        yield return new Argument(null, item);
                    }
                }
            }

            if (key != null)
            {
                yield return new Argument(key, null);
            }
        }
    }
}