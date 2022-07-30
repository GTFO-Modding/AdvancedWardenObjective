using ExtendedWardenEvents.Networking;
using ExtendedWardenEvents.Networking.CommonReplicator;
using ExtendedWardenEvents.Networking.CommonReplicator.Inject;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Events
{
    internal sealed class SetLevelFailCheckEnabledEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.SetLevelFailCheckEnabled;

        protected override void TriggerMaster(WEE_EventData e)
        {
            LevelFailCheckStatusReplicator.SetFailAllowed(e.Enabled);
        }
    }
}
