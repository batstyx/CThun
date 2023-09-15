using CThun.Properties;
using Hearthstone_Deck_Tracker.Hearthstone;
using static HearthDb.CardIds.Collectible;

namespace CThun
{
    public abstract class EffectConfig
    {       
        public string[] ShowOnCardIds => new string[] { Neutral.CthunOG };
        public DisplayMode Player => Settings.Default.PlayerShowCThun;
        public DisplayMode Opponent => Settings.Default.OpponentShowCThun;
        public abstract int GetValue(Player player);
        public Effect Create() => new Effect(this);
    }
}
