﻿using AWO.Modules.WEE;
using AWO.Sessions;

namespace AWO.WEE.Events;

internal sealed class SetLevelFailCheckEnabledEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.SetLevelFailCheckEnabled;

    protected override void TriggerMaster(WEE_EventData e)
    {
        LevelFailUpdateState.SetFailAllowed(e.Enabled);
    }
}
