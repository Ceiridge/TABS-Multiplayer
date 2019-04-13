using System;
using TABS_Multiplayer;

#pragma warning disable CS0626
namespace Landfall.TABS.GameState
{
    class patch_GameStateManager : GameStateManager
    {
        public extern void orig_EnterBattleState();
        public new void EnterBattleState()
        {
            orig_EnterBattleState();
            SocketConnection.gameStarted = true; // Set the local var
            SocketConnection.WriteToOpponent("GSTARTED|true"); // Send the command to the opponent if it's started
        }

        public extern void orig_EnterPlacementState();
        public new void EnterPlacementState()
        {
            orig_EnterPlacementState();
            SocketConnection.gameStarted = false; // Set the local var
            SocketConnection.WriteToOpponent("GSTARTED|false"); // Send the command to the opponent if it's stopped
        }
    }
}
