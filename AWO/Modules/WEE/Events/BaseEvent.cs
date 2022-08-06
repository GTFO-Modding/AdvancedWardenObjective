using AWO.Modules.WEE;
using BepInEx.Core.Logging.Interpolation;
using GameData;
using LevelGeneration;
using Player;
using SNetwork;

namespace AWO.WEE.Events
{
    internal abstract class BaseEvent
    {
        public string Name { get; private set; } = string.Empty;
        protected PlayerAgent LocalPlayer { get; private set; }
        protected static bool IsMaster => SNet.IsMaster;
        protected static bool HasMaster => SNet.HasMaster;
        public abstract WEE_Type EventType { get; }

        public void Setup()
        {
            Name = GetType().Name;
            OnSetup();
        }

        public void Trigger(WEE_EventData e)
        {
            if (!PlayerManager.HasLocalPlayerAgent())
            {
                Logger.Error($"Doesn't have LocalPlayer while triggering {Name} wtf?");
                return;
            }

            LocalPlayer = PlayerManager.GetLocalPlayerAgent();
            TriggerCommon(e);

            if (SNet.IsMaster) TriggerMaster(e);
            else TriggerClient(e);
        }

        protected virtual void OnSetup() { }
        protected virtual void TriggerCommon(WEE_EventData e) { }
        protected virtual void TriggerClient(WEE_EventData e) { }
        protected virtual void TriggerMaster(WEE_EventData e) { }

        protected void LogInfo(string msg) => Logger.Info($"[{Name}] {msg}");
        protected void LogInfo(BepInExInfoLogInterpolatedStringHandler handler)
        {
            if (handler.Enabled)
            {
                Logger.Info($"[{Name}] {handler}");
            }
        }

        protected void LogDebug(string msg) => Logger.Debug($"[{Name}] {msg}");
        protected void LogDebug(BepInExDebugLogInterpolatedStringHandler handler)
        {
            if (handler.Enabled)
            {
                Logger.Debug($"[{Name}] {handler}");
            }
        }

        protected void LogWarning(string msg) => Logger.Warn($"[{Name}] {msg}");
        protected void LogWarning(BepInExWarningLogInterpolatedStringHandler handler)
        {
            if (handler.Enabled)
            {
                Logger.Warn($"[{Name}] {handler}");
            }
        }

        protected void LogError(string msg) => Logger.Error($"[{Name}] {msg}");
        protected void LogError(BepInExErrorLogInterpolatedStringHandler handler)
        {
            if (handler.Enabled)
            {
                Logger.Error($"[{Name}] {handler}");
            }
        }

        public bool TryGetZone(WEE_EventData e, out LG_Zone zone)
        {
            if (!Builder.Current.m_currentFloor.TryGetZoneByLocalIndex(e.DimensionIndex, e.Layer, e.LocalIndex, out zone))
            {
                LogError("Unable to Find Zone from Event Data");
                zone = null;
                return false;
            }
            return true;
        }

        public bool TryGetZoneEntranceSecDoor(WEE_EventData e, out LG_SecurityDoor door)
        {
            if (TryGetZone(e, out var zone))
            {
                return TryGetZoneEntranceSecDoor(zone, out door);
            }
            door = null;
            return false;
        }

        public bool TryGetZoneEntranceSecDoor(LG_Zone zone, out LG_SecurityDoor door)
        {
            if (zone == null)
            {
                LogError("Zone was Null!");
                door = null;
                return false;
            }

            if (zone.m_sourceGate == null)
            {
                LogError("Entrace Gate is Null!");
                door = null;
                return false;
            }

            if (zone.m_sourceGate.SpawnedDoor == null)
            {
                LogError("SpawnedDoor is Null!");
                door = null;
                return false;
            }

            door = zone.m_sourceGate.SpawnedDoor.TryCast<LG_SecurityDoor>();
            return door != null;
        }
    }
}
