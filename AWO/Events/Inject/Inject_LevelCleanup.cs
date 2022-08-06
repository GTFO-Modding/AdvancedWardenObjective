using Globals;
using HarmonyLib;

namespace AWO.Events.Inject;

[HarmonyPatch(typeof(Global), nameof(Global.OnLevelCleanup))]
internal static class Inject_LevelCleanup
{
    private static void Postfix()
    {
        LevelEvents.Invoke_OnLevelCleanup();
    }
}
