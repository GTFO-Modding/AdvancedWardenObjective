using AWO.Modules.WEE;
using AWO.WEE.Replicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.WEE.Events.World
{
    internal sealed class SetLightDataInZoneEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.SetLightDataInZone;

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
}
