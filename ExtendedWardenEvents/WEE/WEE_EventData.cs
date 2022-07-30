using Agents;
using AIGraph;
using GameData;
using LevelGeneration;
using Localization;
using SNetwork;
using System;
using System.Linq;
using UnityEngine;

namespace ExtendedWardenEvents.WEE
{
    public sealed class WEE_EventData
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
        public WEE_CleanupEnemiesData CleanupEnemies { get; set; } = new();
        public WEE_ZoneLightData SetZoneLight { get; set; } = new();
    }

    public sealed class WEE_UpdateFogData
    {
        public bool DoUpdate { get; set; } = false;
        public float FogTransitionDuration { get; set; }
        public uint FogSetting { get; set; }
    }

    public sealed class WEE_SubObjectiveData
    {
        public bool DoUpdate { get; set; } = false;
        public LocalizedText CustomSubObjectiveHeader { get; set; }
        public LocalizedText CustomSubObjective { get; set; }
    }

    public sealed class WEE_ReactorEventData
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

    public sealed class WEE_DoorInteractionData
    {
        public bool Lockdown { get; set; }
        public string LockdownMessage { get; set; }
    }

    public sealed class WEE_CountdownData
    {
        public float Duration { get; set; }
        public LocalizedText TimerText { get; set; } = new();
        public Color TimerColor { get; set; } = Color.red;
    }

    public sealed class WEE_CleanupEnemiesData
    {
        public CleanUpType Type { get; set; } = CleanUpType.Despawn;
        public bool IncludeHibernate { get; set; } = true;
        public bool IncludeAggressive { get; set; } = true;
        public bool IncludeScout { get; set; } = true;
        public uint[] ExcludeEnemyID { get; set; } = Array.Empty<uint>();

        public void DoClear(AIG_CourseNode node)
        {
            if (SNet.IsMaster)
                return;

            if (node == null)
                return;

            if (node.m_enemiesInNode == null)
                return;

            foreach (var enemy in node.m_enemiesInNode)
            {
                bool clear = enemy.AI.Mode switch
                {
                    AgentMode.Agressive => IncludeAggressive,
                    AgentMode.Scout => IncludeScout,
                    AgentMode.Hibernate => IncludeHibernate,
                    _ => true
                };

                if (clear)
                {
                    if (ExcludeEnemyID.Contains(enemy.EnemyDataID))
                    {
                        continue;
                    }

                    switch (Type)
                    {
                        case CleanUpType.Despawn:
                            enemy.m_replicator.Despawn();
                            break;

                        case CleanUpType.Kill:
                            enemy.Damage.IsImortal = false;
                            enemy.Damage.BulletDamage(enemy.Damage.DamageMax, null, default, default, default);
                            break;
                    }
                }
            }
        }

        public enum CleanUpType
        {
            Kill,
            Despawn
        }
    }

    public sealed class WEE_ZoneLightData
    {
        public ModifierType Type { get; set; } = ModifierType.RevertToOriginal;
        public uint LightDataID { get; set; }
        public int Seed { get; set; } = 0; //Random on Zero

        public enum ModifierType : byte
        {
            RevertToOriginal,
            SetZoneLightData
        }
    }

    public enum FilterMode
    {
        Exclude,
        Include,
    }
}
