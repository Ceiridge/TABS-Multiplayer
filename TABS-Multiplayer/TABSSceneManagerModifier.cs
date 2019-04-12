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
            if(SocketConnection.GetIsServer())
            {
                SocketConnection.WriteToOpponent("LOADSCENE|" + scene.name); // Send the current scene name to the opponent
            }

            SocketConnection.WriteToUI("INGAME|true"); // Tell to UI that the player's ingame
        } else if(scene.name == "MainMenu")
        {
            SocketConnection.WriteToUI("INGAME|false");
        }

        SocketConnection.WriteToUI("SHOWMSG|Loading scene " + scene.name); // Debug
        orig_OnSceneLoaded(scene, mode); // Call original function
    }
}
