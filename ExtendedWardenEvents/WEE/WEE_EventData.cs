using GameData;
using LevelGeneration;
using Localization;
using UnityEngine;

namespace ExtendedWardenEvents.WEE
{
    internal sealed class WEE_EventData
    {
        public WEEType Type { get; set; }
        public eWardenObjectiveEventTrigger Trigger { get; set; } = eWardenObjectiveEventTrigger.None;
        public eDimensionIndex DimensionIndex { get; set; } = eDimensionIndex.Reality;
        public LG_LayerType Layer { get; set; } = LG_LayerType.MainLayer;
        public eLocalZoneIndex LocalIndex { get; set; } = eLocalZoneIndex.Zone_0;
        public Vector3 Position { get; set; } = Vector3.zero;
        public float Delay { get; set; } = 0.0f;
        public LocalizedText WardenIntel { get; set; } = new();
        public uint SoundID { get; set; } = 0u;
        public LocalizedText SoundSubtitle { get; set; } = new();
        public uint DialogueID { get; set; } = 0u;

        //Common Updater
        public WEE_SubObjectiveData SubObjective { get; set; } = new();
        public WEE_UpdateFogData Fog { get; set; } = new();

        //Command Specific
        public WEE_ReactorEventData Reactor { get; set; } = new();
        public WEE_CountdownData Countdown { get; set; } = new();
    }

    internal sealed class WEE_UpdateFogData
    {
        public bool DoUpdate { get; set; } = false;
        public float FogTransitionDuration { get; set; }
        public uint FogSetting { get; set; }
    }

    internal sealed class WEE_SubObjectiveData
    {
        public bool DoUpdate { get; set; } = false;
        public LocalizedText CustomSubObjectiveHeader { get; set; }
        public LocalizedText CustomSubObjective { get; set; }
    }

    internal sealed class WEE_ReactorEventData
    {
        public WaveState State { get; set; } = WaveState.Intro;
        public int Wave { get; set; } = 1;
        public float Progress { get; set; } = 0.0f;

        public enum WaveState
        {
            Intro,
            Wave,
            Verify
        }
    }

    internal sealed class WEE_DoorInteractionData
    {
        public bool Lockdown { get; set; }
        public string LockdownMessage { get; set; }
    }

    internal sealed class WEE_CountdownData
    {
        public float Duration { get; set; }
        public LocalizedText TimerText { get; set; } = new();
        public Color TimerColor { get; set; } = Color.red;
    }
}
