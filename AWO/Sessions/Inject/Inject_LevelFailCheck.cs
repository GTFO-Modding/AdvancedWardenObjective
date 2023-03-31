using HarmonyLib;
using Player;

namespace AWO.Networking.CommonReplicator.Inject;

[HarmonyPatch(typeof(WOManager), nameof(WOManager.CheckExpeditionFailed))]
internal static class Inject_LevelFailCheck
{
    public static bool LevelFailAllowed = true;
    public static bool LevelFailWhenAnyPlayerDown = false;

    private static void Postfix(ref bool __result)
    {
        if (!LevelFailAllowed)
        {
            __result = false;
        }
        else
        {
            if (LevelFailWhenAnyPlayerDown && HasAnyDownedPlayer())
            {
                __result = true;
            }
        }
    }

    private static bool HasAnyDownedPlayer()
    {
        bool hasAnyDowned = false;
        var playerCount = PlayerManager.PlayerAgentsInLevel.Count;
        if (playerCount <= 0)
        {
            hasAnyDowned = false;
            return hasAnyDowned;
        }
        for (int i = 0; i < playerCount; i++)
        {
            var player = PlayerManager.PlayerAgentsInLevel[i];
            if (!player.Alive)
            {
                hasAnyDowned = true;
            }
        }

        return hasAnyDowned;
    }
}
