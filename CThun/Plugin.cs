using CThun.Effects;
using CThun.Properties;
using CThun.Views;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.Windows.Controls;

namespace CThun
{
    public class Plugin : IPlugin
    {
        private Settings Settings;
        internal static EventManager Events { get; private set; }

        public string Name => LibraryInfo.Name;
        public string Description => Strings.Get("PluginDescription");
        public string ButtonText => Strings.Get("PluginButtonText");
        public string Author => "batstyx";
        public Version Version => LibraryInfo.Version;
        public MenuItem MenuItem { private set; get; }

        public void OnButtonPress() => SettingsView.Flyout.IsOpen = true;

        private CThun CThun;

        public void OnLoad()
        {
            Settings = Settings.Default;

            MenuItem = new MenuItem { Header = Name };
            MenuItem.Click += (sender, args) => OnButtonPress();

            Events = new EventManager();

            GameEvents.OnGameStart.Add(Events.OnGameStart);
            GameEvents.OnInMenu.Add(Events.OnInMenu);

            GameEvents.OnTurnStart.Add(Events.OnTurnStart);

            GameEvents.OnPlayerPlay.Add(Events.OnPlayerPlay);
            GameEvents.OnPlayerCreateInPlay.Add(Events.OnPlayerCreateInPlay);

            GameEvents.OnOpponentPlay.Add(Events.OnOpponentPlay);
            GameEvents.OnOpponentCreateInPlay.Add(Events.OnOpponentCreateInPlay);

            EffectTracker.AddConfig<CThunAttack>();
            EffectTracker.AddConfig<CThunHealth>();

            CThun = new CThun();
        }

        public void OnUpdate() => CThun.Refresh();

        public void OnUnload()
        {
            if (Settings?.HasChanges ?? false) Settings.Save();

            Events?.Dispose();
            Events = null;

            CThun?.Dispose();
            CThun = null;

            EffectTracker.ClearConfig();
        }
    }
}
