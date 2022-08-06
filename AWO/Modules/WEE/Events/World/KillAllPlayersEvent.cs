using AWO.Modules.WEE;
using Player;

namespace AWO.WEE.Events.World;

internal sealed class KillAllPlayersEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.KillAllPlayers;

    protected override void TriggerMaster(WEE_EventData e)
    {
        foreach (var agent in PlayerManager.PlayerAgentsInLevel)
        {
            agent.Damage.ExplosionDamage(agent.Damage.DamageMax, default, default);
        }
    }
}
