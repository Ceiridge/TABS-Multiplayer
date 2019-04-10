# TABS-Multiplayer
A multiplayer mod for the Totally Accurate Battle Simulator

**Warning:** This mod is very unstable and may contain a lot of bugs. The game experience might also not correspond to your expectations.

Special thanks to [MonoMod](https://github.com/0x0ade/MonoMod)

# How To Install
1. Close TABS
1. Locate your TABS folder (Steam->TABS->Properties->Files->Locate Files)
1. Enter the folder `TotallyAccurateBattleSimulator_Data\Managed`
1. Copy `Assembly-CSharp.dll` and paste it in the folder of the extracted release zip (can be found under the github releases tab)
1. Run `modify.bat`
1. Copy back the `Assembly-CSharp.dll` by replacing it in the Steam folder
1. Copy the `TABS-Multiplayer-UI.exe` into the game's root directory
1. Run the game

# How To Uninstall
1. Close TABS
1. Check TABS for errors (Steam->Tabs->Properties->Files->Check for errors)
1. Delete the `TABS-Multiplayer-UI.exe` file in the game's directory

# How To Mod
1. Clone the repository
1. Change the location of the references to the ones on your computer
1. Make your changes
1. Change the output to Release
1. Copy the content of MonoMod into the Release folder
1. Copy the `Assembly-CSharp.dll` from the game's directory in there (I suggest you to set the reference to there)
1. Execute `MonoMod.exe Assembly-CSharp.dll` in a command prompt
1. Copy the `MONOMODDED_Assembly-CSharp.dll` back to the game's directory and rename it to the original
1. Copy the `TABS-Multiplayer-UI.exe` into the game's root directory
1. Test the game
1. Make a pull request :)
