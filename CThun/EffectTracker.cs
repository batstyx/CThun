using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using CThun.Effects;
using CThun.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker;

namespace CThun
{
    internal class EffectTracker
    {
        public static List<IEffectConfig> Configs { get; } = new List<IEffectConfig>();

        public static void AddConfig<T>() where T : IEffectConfig, new()
        {
            Configs.Add(new T());
        }

        public static void ClearConfig()
        {
            Configs.Clear();
        }

        public IEnumerable<Effect> Effects { get; private set; } = Configs.Select(c => new Effect(c)).ToList();
        private IEnumerable<Effect> PlayList => Effects.Where(e => e.Config.CalculateOn.HasFlag(CalculateOn.Play));
        private IEnumerable<Effect> CreateInPlayList => Effects.Where(e => e.Config.CalculateOn.HasFlag(CalculateOn.CreateInPlay));
        private IEnumerable<Effect> EndOfTurnList => Effects.Where(e => e.Config.CalculateOn.HasFlag(CalculateOn.EndOfTurn));
        private IEnumerable<Effect> RefreshList => Effects.Where(e => e.Config.CalculateOn.HasFlag(CalculateOn.Refresh));

        public ActivePlayer ActivePlayer { get; private set; }
        private Player Player = null;

        public EffectTracker(ActivePlayer activePlayer)
        {
            ActivePlayer = activePlayer;
        }

        internal void GameStart()
        {
            try
            {
#if DEBUG
                Log.Info($"{LibraryInfo.Name} GameStart");
#endif
                SetPlayer();

                foreach (var effect in Effects)
                {
                    effect.Reset();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
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
            try
            {
#if DEBUG
                Log.Info($"{LibraryInfo.Name} Refresh");
#endif
                foreach (var effect in RefreshList)
                {
                    IncrementEffect(effect);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }

        internal void EndOfTurn()
        {
            try
            {
#if DEBUG
                Log.Info($"{LibraryInfo.Name} EndOfTurn");
#endif
                foreach (var effect in EndOfTurnList)
                {
                    IncrementEffect(effect);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }

        internal void Play(Card card)
        {
            try
            {
#if DEBUG
                Log.Info($"{LibraryInfo.Name} Play Card: {card?.Type}+{card?.Race}+{card?.Id}");
#endif
                IncrementEffects(PlayList, card);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void CreateInPlay(Card card)
        {
            try
            {
#if DEBUG
                Log.Info($"{LibraryInfo.Name} CreateInPlay Card: {card?.Type}+{card?.Race}+{card?.Id}");
#endif
                IncrementEffects(CreateInPlayList, card);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void IncrementEffects(IEnumerable<Effect> effects, Card card)
        {
            foreach (var effect in effects)
            {
                if (effect.Config.Condition(card))
                {
                    IncrementEffect(effect);
                }
            }
        }

        private void IncrementEffect(Effect effect)
        {
            effect.Increment(effect.Config.Calculate(Player));
        }
    }
}
