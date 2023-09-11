using WPFLocalizeExtension.Engine;

namespace CThun.Properties
{
    public class Strings
    {
        public static string Get(string key) => LocalizeDictionary.Instance.GetLocalizedObject(LibraryInfo.Name, "Resources", key, LocalizeDictionary.Instance.Culture)?.ToString();
    }
}
