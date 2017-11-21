namespace Inforigami.CLI
{
    using System;
    using System.Collections.Generic;

    public interface ICommandTypeProvider
    {
        IEnumerable<Type> GetCommandTypes();
    }
}