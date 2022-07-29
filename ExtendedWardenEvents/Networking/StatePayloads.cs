using GTFO.API;
using SNetwork;
using System;
using System.Runtime.InteropServices;

namespace ExtendedWardenEvents.Networking
{
    public delegate void OnReceiveDel<S>(ulong sender, uint replicatorID, S newState) where S : struct;

    public static class StatePayloads
    {
        public enum Size
        {
            State10Byte = 10,
            State20Byte = 20,
            State30Byte = 30,
            State40Byte = 40,
            State50Byte = 50,
            State60Byte = 60
        }

        public static Size GetSizeType(int size)
        {
            Size highestSizeCap = Size.State10Byte;
            foreach (var sizeType in Enum.GetValues<Size>())
            {
                int sizeTypeNumber = (int)sizeType;
                if (size >= sizeTypeNumber && (int)highestSizeCap < sizeTypeNumber)
                    highestSizeCap = sizeType;
            }

            return highestSizeCap;
        }

        public static IReplicatorEvent<S> CreateEvent<S>(Size size, string eventName, OnReceiveDel<S> onReceiveCallback) where S : struct
        {
            return size switch
            {
                Size.State10Byte => ReplicatorPayloadWrapper<S, StatePayload10Byte>.Create(eventName, onReceiveCallback),
                Size.State20Byte => ReplicatorPayloadWrapper<S, StatePayload20Byte>.Create(eventName, onReceiveCallback),
                Size.State30Byte => ReplicatorPayloadWrapper<S, StatePayload30Byte>.Create(eventName, onReceiveCallback),
                Size.State40Byte => ReplicatorPayloadWrapper<S, StatePayload40Byte>.Create(eventName, onReceiveCallback),
                Size.State50Byte => ReplicatorPayloadWrapper<S, StatePayload50Byte>.Create(eventName, onReceiveCallback),
                Size.State60Byte => ReplicatorPayloadWrapper<S, StatePayload60Byte>.Create(eventName, onReceiveCallback),
                _ => null,
            };
        }

        public static S Get<S>(byte[] bytes, int bytesLength) where S : struct
        {
            int dataSize = Marshal.SizeOf(typeof(S));
            if (dataSize > bytesLength)
            {
                throw new ArgumentException($"StateData Exceed size of {bytesLength} : Unable to Deserialize", nameof(S));
            }

            IntPtr ptr = Marshal.AllocHGlobal(dataSize);
            Marshal.Copy(bytes, 0, ptr, dataSize);
            S obj = (S)Marshal.PtrToStructure(ptr, typeof(S));
            Marshal.FreeHGlobal(ptr);
            return obj;
        }

        public static void Set<S>(S stateData, int size, ref byte[] payloadBytes) where S : struct
        {
            int dataSize = Marshal.SizeOf(stateData);

            if (dataSize >= size)
            {
                throw new ArgumentException($"StateData Exceed size of {size} : Unable to Serialize", nameof(S));
            }

            byte[] bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(stateData, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            payloadBytes = bytes;
        }
    }

    public interface IReplicatorEvent<S> where S : struct
    {
        public string Name { get; }
        public bool IsRegistered { get; }
        public void Invoke(uint replicatorID, S data);
        public void Invoke(uint replicatorID, S data, SNet_ChannelType channelType);
        public void Invoke(uint replicatorID, S data, SNet_Player target);
        public void Invoke(uint replicatorID, S data, SNet_Player target, SNet_ChannelType channelType);
    }

    public class ReplicatorPayloadWrapper<S, P> : IReplicatorEvent<S> where S : struct where P : struct, IStatePayload 
    {
        public string Name { get; private set; }
        public bool IsRegistered { get; private set; } = false;

        public static IReplicatorEvent<S> Create(string eventName, OnReceiveDel<S> onReceiveCallback)
        {
            var wrapper = new ReplicatorPayloadWrapper<S, P>();
            wrapper.Register(eventName, onReceiveCallback);
            if (wrapper.IsRegistered)
            {
                return wrapper;
            }
            return null;
        }

        public void Register(string eventName, OnReceiveDel<S> onReceiveCallback)
        {
            if (IsRegistered)
                return;

            if (NetworkAPI.IsEventRegistered(eventName))
                return;

            NetworkAPI.RegisterEvent(eventName, (ulong sender, P payload) => { onReceiveCallback?.Invoke(sender, payload.ID, payload.Get<S>()); });
            IsRegistered = true;
            Name = eventName;
        }

        public void Invoke(uint replicatorID, S data)
        {
            

            var payload = new P()
            {
                ID = replicatorID
            };
            payload.Set(data);

            NetworkAPI.InvokeEvent(Name, payload);
        }

        public void Invoke(uint replicatorID, S data, SNet_ChannelType channelType)
        {
            var payload = new P()
            {
                ID = replicatorID
            };
            payload.Set(data);

            NetworkAPI.InvokeEvent(Name, payload, channelType);
        }

        public void Invoke(uint replicatorID, S data, SNet_Player target)
        {
            var payload = new P()
            {
                ID = replicatorID
            };
            payload.Set(data);

            NetworkAPI.InvokeEvent(Name, payload, target);
        }

        public void Invoke(uint replicatorID, S data, SNet_Player target, SNet_ChannelType channelType)
        {
            var payload = new P()
            {
                ID = replicatorID
            };
            payload.Set(data);

            NetworkAPI.InvokeEvent(Name, payload, target, channelType);
        }
    }

    public interface IStatePayload
    {
        public uint ID { get; set; }
        public S Get<S>() where S : struct;
        public void Set<S>(S stateData) where S : struct;
    }

    public struct StatePayload10Byte : IStatePayload
    {
        public const int Size = (int)StatePayloads.Size.State10Byte;
        public uint ID { get => id; set => id = value; }
        private uint id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
        public byte[] PayloadBytes;

        public S Get<S>() where S : struct => StatePayloads.Get<S>(PayloadBytes, Size);
        public void Set<S>(S stateData) where S : struct => StatePayloads.Set(stateData, Size, ref PayloadBytes);
    }

    public struct StatePayload20Byte : IStatePayload
    {
        public const int Size = (int)StatePayloads.Size.State20Byte;

        [field: MarshalAs(UnmanagedType.U4)]
        public uint ID { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
        public byte[] PayloadBytes;

        public S Get<S>() where S : struct => StatePayloads.Get<S>(PayloadBytes, Size);
        public void Set<S>(S stateData) where S : struct => StatePayloads.Set(stateData, Size, ref PayloadBytes);
    }

    public struct StatePayload30Byte : IStatePayload
    {
        public const int Size = (int)StatePayloads.Size.State30Byte;

        [field: MarshalAs(UnmanagedType.U4)]
        public uint ID { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
        public byte[] PayloadBytes;

        public S Get<S>() where S : struct => StatePayloads.Get<S>(PayloadBytes, Size);
        public void Set<S>(S stateData) where S : struct => StatePayloads.Set(stateData, Size, ref PayloadBytes);
    }

    public struct StatePayload40Byte : IStatePayload
    {
        public const int Size = (int)StatePayloads.Size.State40Byte;

        [field: MarshalAs(UnmanagedType.U4)]
        public uint ID { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
        public byte[] PayloadBytes;

        public S Get<S>() where S : struct => StatePayloads.Get<S>(PayloadBytes, Size);
        public void Set<S>(S stateData) where S : struct => StatePayloads.Set(stateData, Size, ref PayloadBytes);
    }

    public struct StatePayload50Byte : IStatePayload
    {
        public const int Size = (int)StatePayloads.Size.State50Byte;

        [field: MarshalAs(UnmanagedType.U4)]
        public uint ID { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
        public byte[] PayloadBytes;

        public S Get<S>() where S : struct => StatePayloads.Get<S>(PayloadBytes, Size);
        public void Set<S>(S stateData) where S : struct => StatePayloads.Set(stateData, Size, ref PayloadBytes);
    }

    public struct StatePayload60Byte : IStatePayload
    {
        public const int Size = (int)StatePayloads.Size.State60Byte;

        [field: MarshalAs(UnmanagedType.U4)]
        public uint ID { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Size)]
        public byte[] PayloadBytes;

        public S Get<S>() where S : struct => StatePayloads.Get<S>(PayloadBytes, Size);
        public void Set<S>(S stateData) where S : struct => StatePayloads.Set(stateData, Size, ref PayloadBytes);
    }
}
