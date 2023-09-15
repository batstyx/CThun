using HearthDb.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CThun
{
    public class EffectContext : NotifyPropertyChanged
    {
        public Effect Attack { get; private set; } 
        public Effect Health { get; private set; }
        public ObservableCollection<Effect> Effects { get; private set; }

        public Orientation Orientation { get => _Orientation; set => SetProperty(ref _Orientation, value); }
        private Orientation _Orientation;
        public Visibility SomeVisibility { get => _SomeVisibility; private set => SetProperty(ref _SomeVisibility, value); }
        private Visibility _SomeVisibility = Visibility.Collapsed;

        public EffectContext()
        {
            Attack = new Effect(EffectConfig.Lookup[GameTag.CTHUN_ATTACK_BUFF.ToString()]);
            Health = new Effect(EffectConfig.Lookup[GameTag.CTHUN_HEALTH_BUFF.ToString()]);
            Effects = new ObservableCollection<Effect>(new Effect[] { Attack, Health });
        }

        public void RefreshVisibility()
        {
            var someVisibility = Visibility.Collapsed;
            foreach (var effect in Effects)
            {
                if (effect.Active)
                {
                    someVisibility = Visibility.Visible;
                    break;
                }
            }
            SomeVisibility = someVisibility;
        }
    }
}
