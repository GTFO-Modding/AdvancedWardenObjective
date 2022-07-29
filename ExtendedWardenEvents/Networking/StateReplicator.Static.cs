using ExtendedWardenEvents.Networking.Inject;
using GTFO.API;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.Networking
{
    public sealed partial class StateReplicator<S> where S : struct
    {
        public static readonly string Name;
        public static readonly string HashName;
        public static readonly string ClientRequestEventName;
        public static readonly string HostSetStateEventName;
        public static readonly string HostSetRecallStateEventName;
        public static readonly int StateSize;
        public static readonly StatePayloads.Size StateSizeType;

        private static readonly IReplicatorEvent<S> ClientRequestEvent;
        private static readonly IReplicatorEvent<S> HostSetStateEvent;
        private static readonly IReplicatorEvent<S> HostSetRecallStateEvent;

        private static readonly Dictionary<uint, StateReplicator<S>> _Replicators = new();

        static StateReplicator()
        {
            Name = typeof(S).Name;

            StateSize = Marshal.SizeOf(typeof(S));
            StateSizeType = StatePayloads.GetSizeType(StateSize);

            using var md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(typeof(S).FullName));
            HashName = Convert.ToBase64String(bytes);
            ClientRequestEventName = $"SRs{Name}-{HashName}";
            HostSetStateEventName = $"SRr{Name}-{HashName}";
            HostSetRecallStateEventName = $"SRre{Name}-{HashName}";

            ClientRequestEvent = StatePayloads.CreateEvent<S>(StateSizeType, ClientRequestEventName, ClientRequestEventCallback);
            HostSetStateEvent =  StatePayloads.CreateEvent<S>(StateSizeType, HostSetStateEventName, HostSetStateEventCallback);
            HostSetRecallStateEvent = StatePayloads.CreateEvent<S>(StateSizeType, HostSetRecallStateEventName, HostSetRecallStateEventCallback);

            Inject_SNet_Capture.OnBufferCapture += Inject_SNet_Capture_OnBufferCapture;
            Inject_SNet_Capture.OnBufferRecalled += Inject_SNet_Capture_OnBufferRecalled;
        }

        private static void Inject_SNet_Capture_OnBufferCapture(eBufferType type)
        {
            foreach (var replicator in _Replicators.Values)
            {
                if (replicator.IsValid)
                    replicator.SaveSnapshot(type);
            }
        }

        private static void Inject_SNet_Capture_OnBufferRecalled(eBufferType type)
        {
            foreach (var replicator in _Replicators.Values)
            {
                if (replicator.IsValid)
                    replicator.RestoreSnapshot(type);
            }
        }

        private StateReplicator() {}

        public static StateReplicator<S> Create(uint replicatorID, LifeTimeType lifeTime, IStateReplicatorHolder<S> holder = null)
        {
            if (replicatorID == 0u)
            {
                Logger.Error("Replicator ID 0 is reserved for empty!");
                return null;
            }

            if (_Replicators.ContainsKey(replicatorID))
            {
                Logger.Error("Replicator ID has already assigned!");
                return null;
            }

            var replicator = new StateReplicator<S>
            {
                ID = replicatorID,
                LifeTime = lifeTime,
                Holder = holder
            };
            return replicator;
        }

        public static void UnloadSessionReplicator()
        {
            List<uint> idsToRemove = new();
            foreach (var replicator in _Replicators.Values)
            {
                if (replicator.LifeTime == LifeTimeType.Session)
                    idsToRemove.Add(replicator.ID);
            }

            foreach (var id in idsToRemove)
            {
                _Replicators.Remove(id);
            }
        }

        private static void ClientRequestEventCallback(ulong sender, uint replicatorID, S newState)
        {
            if (!SNet.IsMaster)
                return;

            if (_Replicators.TryGetValue(replicatorID, out var replicator))
            {
                replicator.SetState(newState);
            }
        }

        private static void HostSetStateEventCallback(ulong sender, uint replicatorID, S newState)
        {
            if (!SNet.HasMaster)
                return;

            if (SNet.Master.Lookup != sender)
                return;

            if (_Replicators.TryGetValue(replicatorID, out var replicator))
            {
                replicator.Internal_ChangeState(newState, isRecall: false);
            }
        }

        private static void HostSetRecallStateEventCallback(ulong sender, uint replicatorID, S newState)
        {
            if (!SNet.HasMaster)
                return;

            if (SNet.Master.Lookup != sender)
                return;

            if (_Replicators.TryGetValue(replicatorID, out var replicator))
            {
                replicator.Internal_ChangeState(newState, isRecall: true);
            }
        }
    }
}
