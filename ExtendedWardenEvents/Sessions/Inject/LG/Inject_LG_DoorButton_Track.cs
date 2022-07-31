using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Sessions.Inject.LG
{
    [HarmonyPatch(typeof(LG_DoorButton))]
    internal static class Inject_LG_DoorButton_Track
    {
        [HarmonyPatch(nameof(LG_DoorButton.Setup))]
        [HarmonyPostfix]
        private static void Post_Spawn(LG_DoorButton __instance)
        {
            LG_Objects.AddDoorButton(__instance);
        }
    }
}
