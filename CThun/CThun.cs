﻿using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using CThun.Properties;
using CThun.Views;
using System;
using System.Windows;

using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Hearthstone_Deck_Tracker.Enums;

namespace CThun
{
    internal class CThun : IDisposable
    {
        private readonly EffectView PlayerView;
        private readonly EffectView OpponentView;

        private EffectTracker PlayerTracker;
        private EffectTracker OpponentTracker;

        private Visibility Visibility => (CoreAPI.Game.IsInMenu && Config.Instance.HideInMenu)
                || CoreAPI.Game.IsBattlegroundsMatch
                || CoreAPI.Game.IsMercenariesMatch
                ? Visibility.Collapsed : Visibility.Visible;

        public CThun()
        {
            PlayerView = CreateView();
            PlayerTracker = CreateTracker(PlayerView, ActivePlayer.Player);
            TrackPlayer(PlayerTracker);

            OpponentView = CreateView();
            OpponentTracker = CreateTracker(OpponentView, ActivePlayer.Opponent);
            TrackOpponent(OpponentTracker);

            Plugin.Events.GameStart += GameStart;
            Plugin.Events.InMenu += InMenu;
        }

        private static EffectView CreateView()
        {
            var view = new EffectView();
            CoreAPI.OverlayCanvas.Children.Add(view);
            return view;
        }

        private static EffectTracker CreateTracker(EffectView view, ActivePlayer activePlayer)
        {
            var tracker = new EffectTracker(activePlayer);
            foreach (var effect in tracker.Effects)
            {
                view.Effects.Add(effect);
            }

            GameEvents.OnGameStart.Add(tracker.GameStart);

            return tracker;
        }

        private static void TrackOpponent(EffectTracker tracker)
        {
            Plugin.Events.OpponentTurnEnd += tracker.EndOfTurn;
            Plugin.Events.OpponentPlay += tracker.Play;
            Plugin.Events.OpponentCreateInPlay += tracker.CreateInPlay;
        }

        private static void TrackPlayer(EffectTracker tracker)
        {
            Plugin.Events.PlayerTurnEnd += tracker.EndOfTurn;
            Plugin.Events.PlayerPlay += tracker.Play;
            Plugin.Events.PlayerCreateInPlay += tracker.CreateInPlay;
        }

        internal void GameStart()
        {
            //Visibility = Visibility.Visible;
        }

        internal void InMenu()
        {
            //Visibility = Config.Instance.HideInMenu ? Visibility.Hidden : Visibility.Visible;
        }

        internal void Refresh()
        {
            PlayerView.Visibility = Visibility;
            OpponentView.Visibility = Visibility;

            if (!(PlayerView.Visibility == Visibility.Visible || OpponentView.Visibility == Visibility.Visible)) return;

            PlayerView.SetLocation(Settings.Default.PlayerTop, 100 - Settings.Default.PlayerLeft);
            PlayerView.Orientation = Settings.Default.PlayerOrientation;

            foreach (var effect in PlayerTracker.Effects)
            {
                effect.Active = CoreAPI.Game.Player.ShowCounter(effect.Config) || CoreAPI.Game.IsInMenu;
            }
            PlayerView.RefreshVisibility();
            PlayerTracker.Refresh();

            OpponentView.SetLocation(Settings.Default.OpponentTop, 100 - Settings.Default.OpponentLeft);
            OpponentView.Orientation = Settings.Default.OpponentOrientation;

            foreach (var effect in OpponentTracker.Effects)
            {
                effect.Active = CoreAPI.Game.Opponent.ShowCounter(effect.Config) || CoreAPI.Game.IsInMenu;
            }
            OpponentView.RefreshVisibility();
            OpponentTracker.Refresh();
        }

        public void Dispose()
        {
            CoreAPI.OverlayCanvas.Children.Remove(OpponentView);
            CoreAPI.OverlayCanvas.Children.Remove(PlayerView);
        }
    }
}