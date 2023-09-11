using System;

namespace CThun
{
    [Serializable]
    public enum DisplayMode
    {
        [Localized("DisplayModeAlways")]
        Always,
        [Localized("DisplayModeAuto")]
        Auto,
        [Localized("DisplayModeNever")]
        Never
    }
}
