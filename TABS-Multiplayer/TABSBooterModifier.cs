using System;
using System.Threading;
using TABS_Multiplayer;

#pragma warning disable CS0626
class patch_TABSBooter : TABSBooter
{
    // Hook the init method
    public extern void orig_Init();
    public new void Init()
    {
        SocketConnection.Init(); // Init the socket manager

        Thread formThread = new Thread(() => StartMPForm()); // Start a windows form UI
        formThread.Start();
        //TABSMPForm.instance = new TABSMPForm();
        //TABSMPForm.instance.ShowDialog();

        orig_Init();
    }

    public void StartMPForm()
    {
        TABSMPForm.instance = new TABSMPForm();
        TABSMPForm.instance.ShowDialog();
    }
}