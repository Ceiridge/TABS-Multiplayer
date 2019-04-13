using System;
using System.Collections.Generic;
using TABS_Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0626
namespace Landfall.TABS
{
    class patch_SandboxGameModeHandler : SandboxGameModeHandler
    {

        public extern void orig_OnMapChanged(int newMap);
        public void OnMapChanged(int newMap)
        {
            if (SocketConnection.GetIsServer())
                orig_OnMapChanged(newMap); // Prevent the client from changing their map with the sidebar
        }
    }
}