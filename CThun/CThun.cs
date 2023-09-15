using CThun.Properties;
using CThun.Views;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Enums;
using System;
using System.Windows;
using System.Windows.Media;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace CThun
{
    internal class CThun : IDisposable
    {
        private readonly EffectView PlayerView;
        private readonly EffectView OpponentView;

        private readonly EffectTracker PlayerTracker;
        private readonly EffectTracker OpponentTracker;

        private Visibility Visibility => (CoreAPI.Game.IsInMenu && Config.Instance.HideInMenu)
                || CoreAPI.Game.IsBattlegroundsMatch
                || CoreAPI.Game.IsMercenariesMatch
                ? Visibility.Collapsed : Visibility.Visible;

        public CThun()
        {
            PlayerView = CreateView();
            PlayerTracker = CreateTracker(PlayerView, ActivePlayer.Player);

            OpponentView = CreateView();
            OpponentTracker = CreateTracker(OpponentView, ActivePlayer.Opponent);

            Settings.Default.PropertyChanged += SettingsChanged;
            SettingsChanged(null, null);
        }

        private static EffectView CreateView()
        {
            var view = new EffectView(new EffectContext());
            CoreAPI.OverlayCanvas.Children.Add(view);
            return view;
        }

        private static EffectTracker CreateTracker(EffectView view, ActivePlayer activePlayer)
        {
            var tracker = new EffectTracker(activePlayer, view.Context.Effects);

            GameEvents.OnGameStart.Add(tracker.GameStart);

            return tracker;
        }

        internal void Refresh()
        {
            PlayerView.Visibility = Visibility;
            OpponentView.Visibility = Visibility;

            if (!(PlayerView.Visibility == Visibility.Visible || OpponentView.Visibility == Visibility.Visible)) return;

            PlayerView.SetLocation(Settings.Default.PlayerTop, 100 - Settings.Default.PlayerLeft);

            foreach (var effect in PlayerTracker.Effects)
            {
                effect.Active = CoreAPI.Game.Player.ShowCounter(effect.Config) || CoreAPI.Game.IsInMenu;
            }
            PlayerView.Context.RefreshVisibility();
            PlayerTracker.Refresh();

            OpponentView.SetLocation(Settings.Default.OpponentTop, 100 - Settings.Default.OpponentLeft);

            foreach (var effect in OpponentTracker.Effects)
            {
                effect.Active = CoreAPI.Game.Opponent.ShowCounter(effect.Config) || CoreAPI.Game.IsInMenu;
            }
            OpponentView.Context.RefreshVisibility();
            OpponentTracker.Refresh();
        }

        private void RefreshPlayerOpacity() => PlayerView.Opacity = Settings.Default.PlayerOpacity / 100;
        private void RefreshPlayerScale() => PlayerView.RenderTransform = new ScaleTransform(Settings.Default.PlayerScale / 100, Settings.Default.PlayerScale / 100);
        private void RefreshPlayerOrientation() => PlayerView.Context.Orientation = Settings.Default.PlayerOrientation;
        private void RefreshOpponentOpacity() => OpponentView.Opacity = Settings.Default.OpponentOpacity / 100;
        private void RefreshOpponentScale() => OpponentView.RenderTransform = new ScaleTransform(Settings.Default.OpponentScale / 100, Settings.Default.OpponentScale / 100);
        private void RefreshOpponentOrientation() => OpponentView.Context.Orientation = Settings.Default.OpponentOrientation;
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
                case nameof(Settings.Default.PlayerOrientation):
                    RefreshPlayerOrientation();
                    break;
                case nameof(Settings.Default.OpponentOpacity):
                    RefreshOpponentOpacity();
                    break;
                case nameof(Settings.Default.OpponentScale):
                    RefreshOpponentScale();
                    break;
                case nameof(Settings.Default.OpponentOrientation):
                    RefreshOpponentOrientation();
                    break;
                default:
                    RefreshPlayerOpacity();
                    RefreshPlayerScale();
                    RefreshPlayerOrientation();
                    RefreshOpponentOpacity();
                    RefreshOpponentScale();
                    RefreshOpponentOrientation();
                    break;
            }           
        }

        public void Dispose()
        {
            CoreAPI.OverlayCanvas.Children.Remove(OpponentView);
            CoreAPI.OverlayCanvas.Children.Remove(PlayerView);
        }
    }
}
