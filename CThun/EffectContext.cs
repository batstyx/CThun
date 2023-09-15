using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CThun
{
    public class EffectContext : NotifyPropertyChanged
    {
        public Effect Attack { get; set; }
        public Effect Health { get; set; }
        public IEnumerable<Effect> AttackAndHealth => new List<Effect>() { Attack, Health };
        public Effect Taunt { get; set; }
        public List<Effect> Effects => new List<Effect>() { Attack, Health, Taunt };

        public Orientation Orientation { get => _Orientation; set => SetProperty(ref _Orientation, value); }
        private Orientation _Orientation;
        public Visibility SomeVisibility { get => _SomeVisibility; set => SetProperty(ref _SomeVisibility, value); }
        private Visibility _SomeVisibility = Visibility.Collapsed;

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
