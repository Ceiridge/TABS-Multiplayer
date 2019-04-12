using Landfall.TABS;
using TABS_Multiplayer;
using UnityEngine.SceneManagement;

#pragma warning disable CS0626
class patch_TABSSceneManager : TABSSceneManager
{
    public static extern void orig_OnSceneLoaded(Scene scene, LoadSceneMode mode);
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode) // Overwrite OnSceneLoaded
    {
        if(scene.name == "GameScene") // Check for the start of the game scene
        {

            SocketConnection.WriteToUI("INGAME|true"); // Tell to UI that the player's ingame
        } else if(scene.name == "MainMenu")
        {
            SocketConnection.WriteToUI("INGAME|false");
        }

        //SocketConnection.WriteToUI("SHOWMSG|Loading scene " + scene.name); // Debug
        //SocketConnection.WriteToUI("DEBUG|Loading new scene"); // Debug
        orig_OnSceneLoaded(scene, mode); // Call original function
    }

    public static extern void orig_LoadMap(MapAsset map);
    public static new void LoadMap(MapAsset map)
    {
        if(SocketConnection.GetIsServer())
        {
            SocketConnection.WriteToOpponent("LOADMAP|" + map.m_mapIndex); // Send the current map's ID
        }
        //SocketConnection.WriteToUI("SHOWMSG|Loading map " + map.m_mapIndex + " " + map.MapName); // Debug
        orig_LoadMap(map); // Call orig function
    }
}
