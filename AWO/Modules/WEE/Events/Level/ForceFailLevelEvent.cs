using AWO.Modules.WEE;
using SNetwork;

namespace AWO.WEE.Events.Level;

internal sealed class ForceFailLevelEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.ForceFailLevel;

    protected override void TriggerMaster(WEE_EventData e)
    {
        SNet.Sync.SessionCommand(eSessionCommandType.TryEndPlaying, 1);
    }
}
