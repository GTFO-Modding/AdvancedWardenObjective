using AWO.Modules.WEE;
using AWO.WEE.Replicators;

namespace AWO.WEE.Events.World;

internal sealed class SetLightDataInZoneEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.SetLightDataInZone;

    protected override void TriggerMaster(WEE_EventData e)
    {
        if (!TryGetZone(e, out var zone))
        {
            LogError("Cannot find zone!");
            return;
        }


        if (e.SetZoneLight != null)
        {
            var setting = e.SetZoneLight;
            var replicator = zone.GetComponent<ZoneLightReplicator>();
            if (setting.Type == WEE_ZoneLightData.ModifierType.SetZoneLightData)
            {
                replicator.SetLightSetting(setting);
            }
            else if (setting.Type == WEE_ZoneLightData.ModifierType.RevertToOriginal)
            {
                replicator.RevertLightData();
            }
        }
    }
}
