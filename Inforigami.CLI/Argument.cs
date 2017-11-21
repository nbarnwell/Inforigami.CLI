namespace Inforigami.CLI
{
    public class Argument
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Argument(string key, string value)
        {
            Key = key;
            Value = value;
        }

        protected bool Equals(Argument other)
        {
            return string.Equals(Key, other.Key) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Argument)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0); }
        }

        public override string ToString()
        {
            return $"Key: \"{Key}\", Value: \"{Value}\"";
        }
    }
}