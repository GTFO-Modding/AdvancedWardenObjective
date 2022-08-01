using ExtendedWardenEvents.Networking;
using GameData;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtendedWardenEvents.WEE.Replicators
{
    internal struct ZoneLightState
    {
        public uint lightData;
        public int lightSeed;
    }

    internal class LightWorker
    {
        public LG_Light Light;
        public Color OrigColor;
        public bool OrigEnabled;
        public float PrefabIntensity;
        public float OrigIntensity;

        public void ApplyLightSetting(LightSettingsDataBlock lightDB, int seed, int subseed)
        {
            var rand = new System.Random(seed);
            for (int i = 0; i< Mathf.Abs(subseed); i++)
            {
                rand.Next();
            }

            var selector = new LightSettingSelector();
            selector.Setup(Light.m_category, lightDB);
            if (selector.TryGetRandomSetting((uint)subseed, out var setting))
            {
                var broken = (float)rand.NextDouble();

                if (broken <= setting.Chance)
                {
                    Light.SetEnabled(false);
                    Light.ChangeColor(Color.black);
                    return;
                }
                else
                {
                    Light.SetEnabled(true);
                    Light.ChangeColor(setting.Color);
                    Light.ChangeIntensity(PrefabIntensity * setting.IntensityMul);

                    //TODO: Animations
                }
            }
        }

        public void Revert()
        {
            Light.ChangeColor(OrigColor);
            Light.ChangeIntensity(OrigIntensity);
            Light.SetEnabled(OrigEnabled);
        }
    }

    internal sealed class ZoneLightReplicator : MonoBehaviour, IStateReplicatorHolder<ZoneLightState>
    {
        public StateReplicator<ZoneLightState> Replicator { get; private set; }
        public LightWorker[] LightsInZone;

        public void Setup(LG_Zone zone)
        {
            Replicator = StateReplicator<ZoneLightState>.Create((uint)zone.ID + 1 /*Zone ID can be start with 0*/, new()
            {
                lightData = 0u
            }, LifeTimeType.Session, this);

            var workers = new List<LightWorker>();
            foreach (var nodes in zone.m_courseNodes)
            {
                foreach (var light in nodes.m_area.GetComponentsInChildren<LG_Light>(false))
                {
                    workers.Add(new LightWorker()
                    {
                        Light = light,
                        PrefabIntensity = light.m_intensity,
                    });
                }
            }
            LightsInZone = workers.ToArray();
        }

        public void Setup_UpdateLightSetting()
        {
            foreach (var worker in LightsInZone)
            {
                worker.OrigColor = worker.Light.m_color;
                worker.OrigIntensity = worker.Light.m_intensity;
                worker.OrigEnabled = worker.Light.gameObject.active;
            }
        }

        void OnDestroy()
        {
            Replicator?.Unload();
        }

        public void SetLightSetting(uint lightID, int seed = 0)
        {
            if (Replicator == null)
                return;

            if (Replicator.IsInvalid)
                return;

            if (seed == 0)
            {
                seed = new System.Random().Next();
            }

            Replicator.SetState(new ZoneLightState()
            {
                lightData = lightID,
                lightSeed = seed
            });
        }

        public void RevertLightData()
        {
            if (Replicator == null)
                return;

            if (Replicator.IsInvalid)
                return;

            Replicator.SetState(new ZoneLightState()
            {
                lightData = 0
            });
        }

        public void OnStateChange(ZoneLightState oldState, ZoneLightState state, bool isRecall)
        {
            if (state.lightData == 0u)
            {
                for (int i = 0; i < LightsInZone.Length; i++)
                {
                    LightsInZone[i].Revert();
                }
            }
            else
            {
                var block = LightSettingsDataBlock.GetBlock(state.lightData);
                if (block == null)
                    return;

                for (int i = 0; i < LightsInZone.Length; i++)
                {
                    LightsInZone[i].ApplyLightSetting(block, state.lightSeed, i);
                }
            }
        }
    }
}
