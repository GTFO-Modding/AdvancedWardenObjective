using Globals;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Networking.Inject
{
    [HarmonyPatch(typeof(Global), nameof(Global.OnLevelCleanup))]
    internal static class Inject_LevelCleanup
    {
        public static event Action OnLevelCleanup;

        private static void Postfix()
        {
            OnLevelCleanup?.Invoke();
        }
    }
}
