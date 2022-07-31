using Expedition;
using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Sessions.Inject.LG
{
    [HarmonyPatch(typeof(LG_HSUActivator_Core))]
    internal static class Inject_LG_HSUActivator_Track
    {
        [HarmonyPatch(nameof(LG_HSUActivator_Core.Start))]
        [HarmonyPostfix]
        private static void Post_Spawn(LG_HSUActivator_Core __instance)
        {
            LG_Objects.AddHSUActivator(__instance);
        }
    }
}
