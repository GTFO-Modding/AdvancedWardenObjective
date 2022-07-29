using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtendedWardenEvents.Networking
{
    public interface IStateReplicatorHolder<S> where S : struct
    {
        public StateReplicator<S> Replicator { get; }
        public void OnStateChange(S oldState, S state, bool isRecall);
        public GameObject gameObject { get; }
    }
}
