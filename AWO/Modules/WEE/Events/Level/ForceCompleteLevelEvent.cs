﻿using AWO.Modules.WEE;
using LevelGeneration;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.WEE.Events.Level
{
    internal sealed class ForceCompleteLevelEvent : BaseEvent
    {
        public override WEE_Type EventType => WEE_Type.ForceCompleteLevel;

        protected override void TriggerMaster(WEE_EventData e)
        {
            WOManager.ForceCompleteObjective(LG_LayerType.MainLayer);
            SNet.Sync.SessionCommand(eSessionCommandType.TryEndPlaying, 2);
        }
    }
}
