using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Inject
{
    [HarmonyPatch(typeof(LG_Factory))]
    internal static class Inject_LG_Factory
    {
        public static event Action PostFactoryDone;

        [HarmonyPatch(nameof(LG_Factory.FactoryDone))]
        [HarmonyPostfix]
        [HarmonyWrapSafe]
        private static void Post_FactoryDone()
        {
            PostFactoryDone?.Invoke();
        }
    }
}
