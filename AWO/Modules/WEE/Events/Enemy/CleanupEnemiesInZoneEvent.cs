using AWO.Modules.WEE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.WEE.Events.Enemy
{
    internal class CleanupEnemiesInZoneEvent : BaseEvent
    {
        public override WEE_Type EventType => WEE_Type.CleanupEnemiesInZone;

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
