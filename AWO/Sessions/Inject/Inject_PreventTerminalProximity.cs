using HarmonyLib;
using LevelGeneration;

namespace AWO.Sessions.Inject;

[HarmonyPatch(typeof(LG_ComputerTerminal), nameof(LG_ComputerTerminal.OnProximityEnter))]
[HarmonyPatch(typeof(LG_ComputerTerminal), nameof(LG_ComputerTerminal.OnProximityExit))]
internal static class Inject_PreventTerminalProximity
{
    private static bool Prefix()
    {
        return !BlackoutState.BlackoutEnabled;
    }
}
