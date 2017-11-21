namespace Inforigami.CLI
{
    using System;
    using System.Collections.Generic;

    public class KnownCommandTypeProvider : ICommandTypeProvider
    {
        private readonly IEnumerable<Type> _types;

        public KnownCommandTypeProvider(IEnumerable<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            _types = types;
        }

        public IEnumerable<Type> GetCommandTypes()
        {
            return _types;
        }
    }
}