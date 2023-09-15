using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using System.Collections.Generic;
using System.Linq;

namespace CThun
{
    internal class EffectTracker
    {
        public IEnumerable<Effect> Effects { get; private set; } 
        public ActivePlayer ActivePlayer { get; private set; }
        private Player Player = null;

        public EffectTracker(ActivePlayer activePlayer, IEnumerable<Effect> effects)
        {
            ActivePlayer = activePlayer;
            Effects = effects;
        }

        internal void GameStart()
        {
            SetPlayer();

            foreach (var effect in Effects)
            {
                effect.Reset();
            }
        }

        private void SetPlayer()
        {
            Player = GetPlayer(ActivePlayer);
        }

        private Player GetPlayer(ActivePlayer activePlayer)
        {
            switch (activePlayer)
            {
                case ActivePlayer.Player:
                    return Core.Game.Player;
                case ActivePlayer.Opponent:
                    return Core.Game.Opponent;
                case ActivePlayer.None:
                default:
                    return null;
            }
        }

        internal void Refresh()
        {
            foreach (var effect in Effects)
            {
                effect.SetValue(effect.Config.GetValue(Player));
            }
        }
    }
}
