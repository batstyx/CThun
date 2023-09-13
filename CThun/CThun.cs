using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using CThun.Properties;
using CThun.Views;
using System;
using System.Windows;

using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Hearthstone_Deck_Tracker.Enums;
using System.Windows.Media;

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

            Settings.Default.PropertyChanged += SettingsChanged;
            SettingsChanged(null, null);
        }

        private void RefreshPlayerOpacity() => PlayerView.Opacity = Settings.Default.PlayerOpacity / 100;
        private void RefreshPlayerScale() => PlayerView.RenderTransform = new ScaleTransform(Settings.Default.PlayerScale / 100, Settings.Default.PlayerScale / 100);
        private void RefreshOpponentOpacity() => OpponentView.Opacity = Settings.Default.OpponentOpacity / 100;
        private void RefreshOpponentScale() => OpponentView.RenderTransform = new ScaleTransform(Settings.Default.OpponentScale / 100, Settings.Default.OpponentScale / 100);
        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var property = e?.PropertyName;
            switch (property)
            {
                case nameof(Settings.Default.PlayerOpacity):
                    RefreshPlayerOpacity();
                    break;
                case nameof(Settings.Default.PlayerScale):
                    RefreshPlayerScale();
                    break;
                case nameof(Settings.Default.OpponentOpacity):
                    RefreshOpponentOpacity();
                    break;
                case nameof(Settings.Default.OpponentScale):
                    RefreshOpponentScale();
                    break;
                default:
                    RefreshPlayerOpacity();
                    RefreshPlayerScale();
                    RefreshOpponentOpacity();
                    RefreshOpponentScale();
                    break;
            }           
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
