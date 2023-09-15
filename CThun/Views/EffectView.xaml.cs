using Hearthstone_Deck_Tracker.API;
using System.Windows.Controls;

namespace CThun.Views
{
    /// <summary>
    /// Interaction logic for EffectView.xaml
    /// </summary>
    public partial class EffectView : UserControl
    {
        public EffectContext Context => DataContext as EffectContext;

        public EffectView(EffectContext context)
        {
            DataContext = context;

            InitializeComponent();
        }

        public void SetLocation(int PercentFromTop, int PercentFromRight)
        {
            Canvas.SetTop(this, Core.OverlayWindow.Height * PercentFromTop / 100);
            Canvas.SetRight(this, Core.OverlayWindow.Width * PercentFromRight / 100);
        }
    }
}
