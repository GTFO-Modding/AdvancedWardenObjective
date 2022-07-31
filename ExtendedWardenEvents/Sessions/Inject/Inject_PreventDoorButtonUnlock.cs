using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Sessions.Inject
{
    [HarmonyPatch(typeof(LG_DoorButton), nameof(LG_DoorButton.OnWeakLockUnlocked))]
    internal static class Inject_PreventDoorButtonUnlock
    {
        private static bool Prefix()
        {
            if (BlackoutState.BlackoutEnabled)
            {
                return false;
            }
            return true;
        }
    }
}
