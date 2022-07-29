using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Events.Enemy
{
    internal class CleanupEnemiesInZoneEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.CleanupEnemiesInZone;

        protected override void TriggerMaster(WEE_EventData e)
        {
            if (!TryGetZone(e, out var zone))
            {
                LogError("Zone is Missing?");
                return;
            }

            var data = e.CleanupEnemies;
            if (data == null)
            {
                LogError("CleanupEnemies Data is null?");
                return;
            }

            foreach (var node in zone.m_courseNodes)
            {
                data.DoClear(node);
            }
        }
    }
}
