using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Sessions.Inject.LG
{
    [HarmonyPatch(typeof(LG_WeakLock))]
    internal static class Inject_LG_WeakLock_Track
    {
        [HarmonyPatch(nameof(LG_WeakLock.Setup))]
        [HarmonyPostfix]
        private static void Post_Spawn(LG_WeakLock __instance)
        {
            LG_Objects.AddWeakLock(__instance);
        }
    }
}
