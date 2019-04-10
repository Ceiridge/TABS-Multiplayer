using System;
using TABS_Multiplayer;

#pragma warning disable CS0626
class patch_TABSBooter : TABSBooter
{
    // Hook the init method
    public extern void orig_Init();
    public new void Init()
    {
        SocketConnection.Init(); // Init the socket manager
        

        orig_Init();
    }
}