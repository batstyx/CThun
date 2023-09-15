using HearthDb.Enums;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Linq;

namespace CThun
{
    internal static class Helper
    {
        public static Entity GetEntity(this Player player, Card card)
        {
            Entity entity = null;
            if (player == Core.Game.Player)
            {
                entity = player.PlayerEntities.FirstOrDefault(x => (x.CardId == card.Id || x.Name == card.Name) && x.Info.OriginalZone != null);
            }
            else if (player == Core.Game.Opponent)
            {
                entity = player.PlayerEntities.FirstOrDefault(x => (x.CardId == card.Id || x.Name == card.Name));
            }
            return entity;
        }
        private static bool CheckCards(this Player player, params string[] cardIds)
        {
            foreach (var cardId in cardIds)
            {
                if (player.GetEntity(Database.GetCardFromId(cardId)) != null) return true;
            }
            return false;
        }
        private static bool ShowCounter(this Player player, DisplayMode mode, params string[] cardIds)
        {
            switch (mode)
            {
                case DisplayMode.Always:
                    return true;
                case DisplayMode.Auto:
                    return player.CheckCards(cardIds);
                case DisplayMode.Never:
                default:
                    return false;
            }
        }
        public static bool ShowCounter(this Player player, EffectConfig config) 
            => player.ShowCounter(config.Player, config.ShowOnCardIds);

        public static int GetCThunAttack(this Player player) 
            => player.GetEntity().GetCThunAttackOrHealth(GameTag.CTHUN_ATTACK_BUFF);
        public static int GetCThunHealth(this Player player) 
            => player.GetEntity().GetCThunAttackOrHealth(GameTag.CTHUN_HEALTH_BUFF);
        public static int GetCThunTaunt(this Player player) 
            => player.GetEntity().GetCThunTaunt();
        private static Entity GetEntity(this Player player)
        {
            if (player == Core.Game.Player) return Core.Game.PlayerEntity;
            else if (player == Core.Game.Opponent) return Core.Game.OpponentEntity;
            return null;
        }
        private static int GetCThunTaunt(this Entity player) 
            => player?.HasTag(GameTag.CTHUN_TAUNT_BUFF) ?? false ? player.GetTag(GameTag.CTHUN_TAUNT_BUFF) : 0;
        private static int GetCThunAttackOrHealth(this Entity player, GameTag tag) 
            => (player?.HasTag(tag) ?? false ? player.GetTag(tag) + 6 : 6);
    }
}
