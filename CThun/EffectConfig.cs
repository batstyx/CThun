using CThun.Properties;
using Hearthstone_Deck_Tracker.Hearthstone;
using System.Collections.Generic;
using static HearthDb.CardIds.Collectible;

namespace CThun
{
    public abstract class EffectConfig
    {
        public static Dictionary<string, EffectConfig> Lookup { get; } = new Dictionary<string, EffectConfig>();
        public static void AddToLookup<T>(string key) where T : EffectConfig, new()
        {
            Lookup.Add(key, new T());
        }

        public string[] ShowOnCardIds => new string[] { Neutral.CthunOG };
        public DisplayMode Player => Settings.Default.PlayerShowCThun;
        public DisplayMode Opponent => Settings.Default.OpponentShowCThun;
        public abstract int GetValue(Player player);
    }
}
