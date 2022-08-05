using Globals;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Events.Inject
{
    [HarmonyPatch(typeof(Global), nameof(Global.OnLevelCleanup))]
    internal static class Inject_LevelCleanup
    {
        private static void Postfix()
        {
            LevelEvents.Invoke_OnLevelCleanup();
        }
    }
}
