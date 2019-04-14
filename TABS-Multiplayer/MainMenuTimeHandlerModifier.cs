using System;

#pragma warning disable CS0626
class patch_MainMenuTimeHandler : MainMenuTimeHandler
{
    public extern void orig_Freeze();
    public new void Freeze(){} // Prevent the main menu from slowing down time
}