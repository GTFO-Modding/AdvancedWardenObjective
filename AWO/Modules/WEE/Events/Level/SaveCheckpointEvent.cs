﻿using AWO.Modules.WEE;
using SNetwork;

namespace AWO.WEE.Events.Level;

internal sealed class SaveCheckpointEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.SaveCheckpoint;

    protected override void TriggerMaster(WEE_EventData e)
    {
        CheckpointManager.StoreCheckpoint(LocalPlayer.EyePosition);
        SNet.Capture.CaptureGameState(eBufferType.Checkpoint);
    }
}
