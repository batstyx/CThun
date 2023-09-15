namespace CThun
{
    public class Effect : NotifyPropertyChanged
    {
        public EffectConfig Config { get; }
        public int Value { get => _Value; set => SetProperty(ref _Value, value); }
        private int _Value;
        public bool Active { get => _Active; set => SetProperty(ref _Active, value); }
        private bool _Active;

        public Effect()
        {
        }

        public Effect(EffectConfig config)
        {
            Config = config;
        }
        
        public void Reset() => Value = 0;        
    }
}
