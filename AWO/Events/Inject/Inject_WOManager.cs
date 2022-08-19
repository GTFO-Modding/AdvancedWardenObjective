using HarmonyLib;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Events.Inject;

[HarmonyPatch(typeof(WOManager), nameof(WOManager.SetupWardenObjectiveLayer))]
internal static class Inject_WOManager
{
    private static void Postfix(LG_LayerType layer, int chainIndex)
    {
        WOEvents.Invoke_OnSetup(layer, chainIndex);
    }
}
