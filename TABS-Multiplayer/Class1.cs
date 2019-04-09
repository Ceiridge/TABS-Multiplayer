using System;

#pragma warning disable CS0626
namespace Landfall.TABS
{
    // Test Patch
    class patch_EscapeMenuHandler : EscapeMenuHandler
    {
        public extern void orig_GoToMainMenu();

        public new void GoToMainMenu()
        {
            Console.WriteLine("Hook Test!");
        }
    }
}
