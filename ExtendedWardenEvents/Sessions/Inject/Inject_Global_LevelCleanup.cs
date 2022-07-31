using Globals;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Sessions.Inject
{
    [HarmonyPatch(typeof(Global), nameof(Global.OnLevelCleanup))]
    internal static class Inject_Global_LevelCleanup
    {
        private static void Postfix()
        {
            BlackoutState.LevelCleanup();
            LevelFailUpdateState.LevelCleanup();
            LG_Objects.Clear();
        }
    }
}
