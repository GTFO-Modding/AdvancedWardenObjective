using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Sessions.Inject.LG
{
    [HarmonyPatch(typeof(LG_ComputerTerminal))]
    internal static class Inject_LG_ComputerTerminal_Track
    {
        [HarmonyPatch(nameof(LG_ComputerTerminal.Setup))]
        [HarmonyPostfix]
        private static void Post_Spawn(LG_ComputerTerminal __instance)
        {
            LG_Objects.AddTerminal(__instance);
        }
    }
}
