using AWO.Modules.WEE;
using AWO.Networking;
using AWO.Networking.CommonReplicator.Inject;
using AWO.Sessions;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.WEE.Events
{
    internal sealed class SetLevelFailCheckEnabledEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.SetLevelFailCheckEnabled;

        protected override void TriggerMaster(WEE_EventData e)
        {
            LevelFailUpdateState.SetFailAllowed(e.Enabled);
        }
    }
}
