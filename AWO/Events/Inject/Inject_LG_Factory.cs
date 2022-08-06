using HarmonyLib;
using LevelGeneration;

namespace AWO.Events.Inject;

[HarmonyPatch(typeof(LG_Factory))]
internal static class Inject_LG_Factory
{
    [HarmonyPatch(nameof(LG_Factory.FactoryDone))]
    [HarmonyPrefix]
    [HarmonyWrapSafe]
    private static void Pre_FactoryDone()
    {
        LevelEvents.Invoke_OnLevelBuildDone();
    }

    [HarmonyPatch(nameof(LG_Factory.FactoryDone))]
    [HarmonyPostfix]
    [HarmonyWrapSafe]
    private static void Post_FactoryDone()
    {
        LevelEvents.Invoke_OnLevelBuildDoneLate();
    }
}
