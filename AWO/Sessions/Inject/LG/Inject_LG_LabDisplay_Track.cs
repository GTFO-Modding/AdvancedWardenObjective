using Expedition;
using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Sessions.Inject.LG
{
    [HarmonyPatch(typeof(LG_LabDisplay))]
    internal static class Inject_LG_LabDisplay_Track
    {
        [HarmonyPatch(nameof(LG_LabDisplay.GenerateText))]
        [HarmonyPatch(new Type[] { typeof(int), typeof(SubComplex) })]
        [HarmonyPostfix]
        private static void Post_Spawn(LG_LabDisplay __instance)
        {
            LG_Objects.AddLabDisplay(__instance);
        }
    }
}
