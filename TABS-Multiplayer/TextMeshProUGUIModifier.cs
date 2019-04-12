using TMPro;
using UnityEngine;

#pragma warning disable CS0626
namespace TMPRO
{
    class patch_TextMeshProUGUI : TextMeshProUGUI
    {
        private static float waitTime = 0f, interval = 0.0166f; // Interval set to 16.6 ms (~60fps)

        public extern void orig_LateUpdate();
        public void LateUpdate() // Hook the LateUpdate method for a thread-safe tick hook
        {
            if(Time.time > waitTime) // Wait for the interval - This is to make sure that the mod's stuff doesn't get called too often
            {
                waitTime += interval;


            }

            orig_LateUpdate(); // Call the original method
        }
    }
}
