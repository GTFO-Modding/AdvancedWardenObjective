using LevelGeneration;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Events
{
    internal sealed class ForceCompleteLevelEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.ForceCompleteLevel;

        protected override void TriggerMaster(WEE_EventData e)
        {
            WOManager.ForceCompleteObjective(LG_LayerType.MainLayer);
            SNet.Sync.SessionCommand(eSessionCommandType.TryEndPlaying, 2);
        }
    }
}
