using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Events
{
    internal sealed class SaveCheckpointEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.SaveCheckpoint;

        protected override void TriggerMaster(WEE_EventData e)
        {
            CheckpointManager.StoreCheckpoint(LocalPlayer.EyePosition);
            SNet.Capture.CaptureGameState(eBufferType.Checkpoint);
        }
    }
}
