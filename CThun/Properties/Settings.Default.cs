namespace CThun.Properties
{
    public sealed partial class Settings
    {
        private int GetDefaultIntValue(string key) => int.Parse(Properties[key].DefaultValue.ToString());
        private int GetDefaultDblValue(string key) => int.Parse(Properties[key].DefaultValue.ToString());
        public int DefaultPlayerLeft => GetDefaultIntValue("PlayerLeft");
        public int DefaultPlayerTop => GetDefaultIntValue("PlayerTop");
        public int DefaultOpponentLeft => GetDefaultIntValue("OpponentLeft");
        public int DefaultOpponentTop => GetDefaultIntValue("OpponentTop");
        public int DefaultPlayerOpacity => GetDefaultDblValue("PlayerOpacity");
        public int DefaultPlayerScale => GetDefaultDblValue("PlayerScale");
        public int DefaultOpponentOpacity => GetDefaultDblValue("OpponentOpacity");
        public int DefaultOpponentScale => GetDefaultDblValue("OpponentScale");
        public void ResetPlayerPosition()
        {
            PlayerLeft = DefaultPlayerLeft;
            PlayerTop = DefaultPlayerTop;
        }
        public void ResetPlayerDisplay()
        {
            PlayerOpacity = DefaultPlayerOpacity; 
            PlayerScale = DefaultPlayerScale;
        }
        public void ResetOpponentPosition()
        {
            OpponentLeft = DefaultOpponentLeft;
            OpponentTop = DefaultOpponentTop;
        }
        public void ResetOpponentDisplay()
        {
            OpponentOpacity = DefaultOpponentOpacity;
            OpponentScale = DefaultOpponentScale;
        }
    }
}
