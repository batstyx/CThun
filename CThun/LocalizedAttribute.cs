using CThun.Properties;
using System;

namespace CThun
{
    public class LocalizedAttribute : Attribute
    {
        public string Key { get; }
        public string Description { get; }

        public LocalizedAttribute(string key)
        {
            Key = key;
            Description = Strings.Get(Key);
        }
    }
}
