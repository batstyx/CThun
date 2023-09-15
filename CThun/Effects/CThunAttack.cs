using Hearthstone_Deck_Tracker.Hearthstone;

namespace CThun.Effects
{
    internal class CThunAttack : EffectConfig
    {
        public override int GetValue(Player player) => player.GetCThunAttack();
    }
}
