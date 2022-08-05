using AWO.Modules.WEE;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.WEE.Events.Level
{
    internal sealed class ForceFailLevelEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.ForceFailLevel;

        protected override void TriggerMaster(WEE_EventData e)
        {
            SNet.Sync.SessionCommand(eSessionCommandType.TryEndPlaying, 1);
        }
    }
}
