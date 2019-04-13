using Landfall.TABS;
using TABS_Multiplayer;
using UnityEngine;

#pragma warning disable CS0626
namespace TABS_Multiplayer
{
    class TickCoroutine
    {
        private static float waitTime = 0f, interval = 0.0166f;

        public static void Tick() // A thread safe repeating method
        {
            if (Time.time > waitTime) // Wait until the interval has passed (~60tps)
            {
                waitTime += interval;

                if (SocketConnection.switchScene)
                {
                    SocketConnection.switchScene = false;
                    TABSSceneManager.LoadScene(SocketConnection.newScene, true); // Instantly load the new scene
                }

                if (SocketConnection.switchMap)
                {
                    //SocketConnection.WriteToUI("SHOWMSG|Loading a map!"); // Debug
                    SocketConnection.switchMap = false;
                    CampaignPlayerDataHolder.StartedPlayingSandbox(); // Change the game state
                    TABSSceneManager.LoadMap(GetMap(SocketConnection.newMap)); // Load the new map
                }
            }
        }

        private static MapAsset GetMap(int index) // Get the map by the index id
        {
            foreach(MapAsset map in LandfallUnitDatabase.GetDatabase().Maps)
            {
                if (map.m_mapIndex == index)
                    return map;
            }
            return LandfallUnitDatabase.GetDatabase().Maps[0]; // Return the first one if none is found (illegal state)
        }
    }
}

// Several hooks for reliable ticking
class patch_GeneralInput : GeneralInput 
{
    public extern void orig_Update();
    public void Update()
    {
        TickCoroutine.Tick();
        orig_Update();
    }
}
class patch_SandboxGameModeHandler : SandboxGameModeHandler
{
    public extern void orig_Update();
    public void Update()
    {
        TickCoroutine.Tick();
        orig_Update();
    }
}
class patch_CameraAbilityPossess : CameraAbilityPossess
{
    public extern void orig_LateUpdate();
    public void LateUpdate()
    {
        TickCoroutine.Tick();
        orig_LateUpdate();
    }
}
class patch_PlayerInput : PlayerInput
{
    public extern void orig_Update();
    public void Update()
    {
        TickCoroutine.Tick();
        orig_Update();
    }
}