using Hearthstone_Deck_Tracker.Hearthstone;
using CThun.Properties;
using System;
using static HearthDb.CardIds.Collectible;

namespace CThun.Effects
{
    internal class CThunAttack : IEffectConfig
    {
        public string Name => Strings.Get("CThunAttackEffectName");
        public string[] ShowOnCardIds => new string[] { Neutral.CthunOG };
        public DisplayMode Player => Settings.Default.PlayerShowCThun;
        public DisplayMode Opponent => Settings.Default.OpponentShowCThun;
        public Predicate<Card> Condition => null;
        public Func<Player, int> Calculate => player => player.GetCThunAttack();
        public CalculateOn CalculateOn => CalculateOn.Refresh;
    }
}
