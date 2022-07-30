using ExtendedWardenEvents.Networking;
using ExtendedWardenEvents.WEE.Inject;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Events
{
    internal struct LevelFailCheck
    {
        public bool failAllowed;
    }

    internal sealed class SetLevelFailCheckEnabled : BaseEvent
    {
        public override WEEType EventType => WEEType.SetLevelFailCheckEnabled;
        private StateReplicator<LevelFailCheck> _Replicator = StateReplicator<LevelFailCheck>.Create(1u, new() { failAllowed = true }, LifeTimeType.Permanent);

        protected override void OnSetup()
        {
            LG_Factory.add_OnFactoryBuildStart(new Action(() =>
            {
                _Replicator.SetState(new()
                {
                    failAllowed = true
                });
            }));
            _Replicator.OnStateChanged += _Replicator_OnStateChanged;
        }

        private void _Replicator_OnStateChanged(LevelFailCheck old, LevelFailCheck state, bool isRecall)
        {
            Inject_LevelFailCheck.LevelFailAllowed = state.failAllowed;
        }

        protected override void TriggerMaster(WEE_EventData e)
        {
            _Replicator.SetState(new()
            {
                failAllowed = e.Enabled
            });
        }
    }
}
