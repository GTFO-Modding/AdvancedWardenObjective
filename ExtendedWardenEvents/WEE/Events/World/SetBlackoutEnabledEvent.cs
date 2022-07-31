using ExtendedWardenEvents.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Events.World
{
    internal sealed class SetBlackoutEnabledEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.SetBlackoutEnabled;

        protected override void TriggerMaster(WEE_EventData e)
        {
            BlackoutState.SetEnabled(e.Enabled);
        }
    }
}
