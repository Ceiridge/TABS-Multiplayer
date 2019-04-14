# TABS-Multiplayer
A multiplayer mod for the *Totally Accurate Battle Simulator (Totally Accurate Battle Simulator Online Multiplayer)*

**Warning:** This mod is very unstable and may contain a lot of bugs. The game experience might also not correspond to your expectations. **MacOS and Linux is not supported!**

Special thanks to [MonoMod](https://github.com/0x0ade/MonoMod)

# How To Install
**WARNING! UNINSTALL BEFORE UPDATING!!!**
1. Close TABS
1. Download the latest release zip and extract it in a new folder somewhere
1. Locate your TABS folder (Steam->TABS->Properties->Files->Locate Files)
1. Copy the path (should look similar/equal to this: `C:\Program Files (x86)\Steam\steamapps\common\Totally Accurate Battle Simulator`)
1. Run `modify.bat` and paste the path, press *ENTER*
1. Open the TABS folder again, then open `TotallyAccurateBattleSimulator_Data`, then open `Managed`
1. Copy the `Assembly-CSharp.dll` from the folder you created earlier and paste & replace it in the TABS folder you've just navigated to
1. Copy the `TABS-Multiplayer-UI.exe` from the folder you created earlier into the game's root directory (the folder you opened at the beginning, near the  `TotallyAccurateBattleSimulator.exe`)
1. Run `TABS-Multiplayer-UI.exe` to make sure, no AV interferes with the program, **then close it again!** (Especially if you have Avast or Avira and you are experiencing problems, you can add it to the exemptions there)
1. Open your advanced firewall settings and add a new inbound rule that allows every TCP + UDP 8042 connection
1. Run the game

# How To Play
1. Open TABS
1. Click `OK` on the welcome message and wait through the loading screens
1. If you want to be the host: Port-forward the port 8042 (TCP + UDP) in your router settings; As the opponent, you need to connect to the WAN ip of the host (https://checkip.amazonaws.com) (needs to be opened in the host's browser)
1. As a hoster, press `Host`, as an opponent, press `Connect` in the multiplayer UI
1. Make sure not to play in fullscreen
1. Close the `You can now start a sandbox game` dialog (as the host)
1. As the host, start a sandbox game with your favorite map. The opponent will load the same one
1. Place your units until the host presses `Start`
1. A screenshare-window will overlap the TABS window that streams the host's TABS window (This is why I recommend that the player with the best internet connection should be the host)

# How To Update
1. Uninstall
1. Install

# How To Uninstall
1. Close TABS
1. Check TABS for errors (Steam->Tabs->Properties->Files->Check for errors)
1. Delete the `TABS-Multiplayer-UI.exe` file in the game's directory (optional)

# How To Mod The Mod
1. Clone the repository
1. Change the location of the references to the ones on your computer
1. Copy the dlls of MonoMod into the `bin\Release` folder (create the folder at first if necessary)
1. Copy the `Assembly-CSharp.dll` from the game's directory in there, too (I suggest you to set the reference to there)
1. Make your changes
1. Change the output type to **Release**
1. Execute `MonoMod.exe Assembly-CSharp.dll` in a command prompt
1. Copy the `MONOMODDED_Assembly-CSharp.dll` back to the game's directory and rename it to the original
1. Copy the `TABS-Multiplayer-UI.exe` from the UI-project `bin\Releases` into the game's root directory
1. Test the game
1. Make a pull request :)
