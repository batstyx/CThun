using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using System;

namespace CThun
{
    public interface IEffectConfig
    {
        string Name { get; }
        string[] ShowOnCardIds { get; }
        DisplayMode Player { get; }
        DisplayMode Opponent { get; }
        Predicate<Card> Condition { get; }
        Func<Player, int> Calculate { get; }
        CalculateOn CalculateOn { get; }
    }
}
