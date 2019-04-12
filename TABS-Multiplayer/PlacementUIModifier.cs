using TABS_Multiplayer;

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
    }
}
