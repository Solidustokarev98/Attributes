namespace ParamsAttribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IniConfigAttribute : Attribute
    {
        public string FileName { get; }

        public IniConfigAttribute(string FileName)
        {
            this.FileName = FileName;
        }
    }
}