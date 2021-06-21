namespace PartyMaker.Common.Impl
{
    public class NamedParameter
    {
        public NamedParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public object Value { get; }
    }
}
