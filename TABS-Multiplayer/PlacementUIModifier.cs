﻿using TABS_Multiplayer;

#pragma warning disable CS0626
namespace Landfall.TABS
{
    class patch_PlacementUI : PlacementUI
    {
        public extern bool orig_CanStartGame();
        public bool CanStartGame()
        {
            if (!SocketConnection.GetIsServer()) // Prevent the client from starting the game, since the host is in charge
                return false;
            return orig_CanStartGame();
        }

        public extern void orig_Update();
        public void Update()
        {
            TickCoroutine.Tick(); // Again another hook that calls that tick method - just to make sure it really gets called.
            orig_Update();
        }

        public extern void orig_ClearRed();
        public new void ClearRed()
        {
            if (!SocketConnection.GetIsServer()) // Prevent the opponent from clearing the host's warriors
                return;
            orig_ClearRed(); // The host can clear both areas by the way
        }

        public extern void orig_ClearBlue();
        public new void ClearBlue()
        {
            orig_ClearBlue();
            // TODO: Send a clear command
        }
    }
}
