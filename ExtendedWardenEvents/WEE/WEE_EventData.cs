using GameData;
using LevelGeneration;
using Localization;

namespace ExtendedWardenEvents.WEE
{
    internal sealed class WEE_EventData
    {
        public WEEType Type { get; set; }
        public eWardenObjectiveEventTrigger Trigger { get; set; }
        public eDimensionIndex DimensionIndex { get; set; }
        public LG_LayerType Layer { get; set; }
        public eLocalZoneIndex LocalIndex { get; set; }
        public float Delay { get; set; }
        public LocalizedText WardenIntel { get; set; } = new();
        public uint SoundID { get; set; }
        public LocalizedText SoundSubtitle { get; set; } = new();
        public uint DialogueID { get; set; }

        //Common Updater
        public WEE_SubObjectiveData SubObjective { get; set; } = new();
        public WEE_UpdateFogData Fog { get; set; } = new();

        //Command Specific
        public WEE_ReactorEventData Reactor { get; set; } = new();
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
    }
}
