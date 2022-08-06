using HarmonyLib;

namespace AWO.Networking.CommonReplicator.Inject;

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
