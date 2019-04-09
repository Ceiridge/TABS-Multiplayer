using Landfall.TABS;
using Landfall.TABS.Workshop;
using System;

#pragma warning disable CS0626

// Prevent the client from sending false analytics data
class patch_TABSAnalytics : TABSAnalytics
{
    public static extern void orig_CampaignBattleEnded();
    public new static void CampaignBattleEnded(bool didWin, TABSCampaignLevelAsset map) { } // Completely override the method

    public static extern void orig_SandboxBattleEnded();
    public new static void SandboxBattleEnded(bool redWin, MapAsset map) { } // Completely override the method
}
