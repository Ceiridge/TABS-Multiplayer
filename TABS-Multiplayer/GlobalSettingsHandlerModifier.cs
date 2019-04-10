using UnityEngine;

#pragma warning disable CS0626
class patch_GlobalSettingsHandler : GlobalSettingsHandler
{
    public static extern FullScreenMode orig_GetFullScreenMode(int value);
    public static new FullScreenMode GetFullScreenMode(int value)
    {
        return FullScreenMode.Windowed; // Always return windowed to force the mode
    }
}