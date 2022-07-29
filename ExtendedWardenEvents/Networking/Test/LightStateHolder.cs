using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtendedWardenEvents.Networking.Test
{
    internal struct LightState
    {
        public float intensity;
        public int isOn;
    }

    internal class LightStateHolder : MonoBehaviour, IStateReplicatorHolder<LightState>
    {
        void Start()
        {
            Replicator = StateReplicator<LightState>.Create(1, LifeTimeType.Permanent, this);
        }

        float updaterTimer = 0;
        void Update()
        {
            if (updaterTimer < Time.time && SNet.IsMaster)
            {
                updaterTimer = Time.time + 3.0f;
                Replicator.SetState(new LightState()
                {
                    intensity = Time.time,
                    isOn = Replicator.State.isOn+1
                });
            }
        }

        void OnDestroy()
        {
            Replicator.Unload();
        }

        public StateReplicator<LightState> Replicator { get; private set; }

        public void OnStateChange(LightState oldState, LightState state, bool isRecall)
        {
            Logger.Error($"{state.isOn} {state.intensity}");
        }
    }
}
