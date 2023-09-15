using Hearthstone_Deck_Tracker.Hearthstone;

namespace CThun.Effects
{
    internal class CThunTaunt : EffectConfig
    {
        public override int GetValue(Player player) => player.GetCThunTaunt();
    }
}
