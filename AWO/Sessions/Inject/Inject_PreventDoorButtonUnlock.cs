using HarmonyLib;
using LevelGeneration;

namespace AWO.Sessions.Inject;

[HarmonyPatch(typeof(LG_DoorButton), nameof(LG_DoorButton.OnWeakLockUnlocked))]
internal static class Inject_PreventDoorButtonUnlock
{
    private static bool Prefix()
    {
        return !BlackoutState.BlackoutEnabled;
    }
}
