using System;

namespace Inforigami.CLI
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class CommandAliasAttribute : Attribute
    {
        public string Alias { get; }

        public CommandAliasAttribute(string alias)
        {
            Alias = alias;
        }
    }
}