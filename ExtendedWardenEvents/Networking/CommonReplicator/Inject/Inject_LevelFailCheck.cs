using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Networking.CommonReplicator.Inject
{
    [HarmonyPatch(typeof(WOManager), nameof(WOManager.CheckExpeditionFailed))]
    internal static class Inject_LevelFailCheck
    {
        public static bool LevelFailAllowed = true;

        private static void Postfix(ref bool __result)
        {
            if (!LevelFailAllowed)
            {
                __result = false;
            }
        }
    }
}
