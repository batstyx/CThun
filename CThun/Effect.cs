using Hearthstone_Deck_Tracker.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CThun
{
    public class Effect : NotifyPropertyChanged
    {
        public EffectConfig Config { get; }
        public int Value { get => _Value; private set => SetProperty(ref _Value, value); }
        private int _Value;
        public bool Active { get => _Active; set => SetProperty(ref _Active, value); }
        private bool _Active;

        public Effect(EffectConfig config)
        {
            Config = config;
        }

        public int SetValue(int value)
        {
            Value = value;
            return Value;
        }
        
        public void Reset() => Value = 0;        
    }
}
