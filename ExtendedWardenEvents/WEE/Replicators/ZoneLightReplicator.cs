using BepInEx.IL2CPP.Utils;
using ExtendedWardenEvents.Networking;
using GameData;
using LevelGeneration;
using System;
using System.Collections;
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
        public float duration;
    }

    internal struct LightTransitionData
    {
        public float startIntensity;
        public float endIntensity;
        public Color startColor;
        public Color endColor;
        public Mode endMode;
        public int endModeSeed;

        public enum Mode
        {
            Enabled,
            Disabled,
            Flickering
        }
    }

    internal class LightWorker
    {
        public LG_Zone OwnerZone;
        public LG_Light Light;
        public Color OrigColor;
        public bool OrigEnabled;
        public float PrefabIntensity;
        public float OrigIntensity;
        public Coroutine LightAnimationRoutine;

        public void ApplyLightSetting(LightSettingsDataBlock lightDB, float duration, int seed, int subseed)
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
                    OwnerZone.StartCoroutine(LightTransition(new()
                    {
                        startColor = Light.m_color,
                        endColor = Color.black,
                        startIntensity = Light.m_intensity,
                        endIntensity = 0.0f,
                        endMode = LightTransitionData.Mode.Disabled
                    }, duration));
                }
                else
                {
                    var mode = ((float)rand.NextDouble() <= setting.ChanceBroken)
                        ? LightTransitionData.Mode.Flickering : LightTransitionData.Mode.Enabled;

                    OwnerZone.StartCoroutine(LightTransition(new()
                    {
                        startColor = Light.m_color,
                        endColor = setting.Color,
                        startIntensity = Light.m_intensity,
                        endIntensity = PrefabIntensity * setting.IntensityMul,
                        endMode = mode,
                        endModeSeed = rand.Next()
                    }, duration));
                }
            }
        }

        private IEnumerator LightTransition(LightTransitionData data, float duration)
        {
            var time = 0.0f;
            var yielder = new WaitForFixedUpdate();
            while (time <= duration)
            {
                time += Time.fixedDeltaTime;

                var progress = time / duration;
                Light.ChangeColor(Color.Lerp(data.startColor, data.endColor, progress));
                Light.ChangeIntensity(Mathf.Lerp(data.startIntensity, data.endIntensity, progress));
                yield return yielder;
            }

            StopAnimation();

            switch (data.endMode)
            {
                case LightTransitionData.Mode.Enabled:
                    Light.SetEnabled(true);
                    break;

                case LightTransitionData.Mode.Disabled:
                    Light.SetEnabled(false);
                    break;

                case LightTransitionData.Mode.Flickering:
                    Light.SetEnabled(true);
                    LightAnimationRoutine = OwnerZone.StartCoroutine(LightAnimation(data.endModeSeed));
                    break;
            }
        }

        private IEnumerator LightAnimation(int seed)
        {
            var rand = new System.Random(seed);
            var yielder = new WaitForFixedUpdate();
            while (true)
            {
                float time = 0.0f;
                float duration = ((float)rand.NextDouble() * (3.5f - 1.0f)) + 1.0f;
                float speed = ((float)rand.NextDouble() * (4.0f - 1.5f)) + 1.5f;
                switch (rand.Next(0, 2))
                {
                    case 0:
                        while(time <= duration)
                        {
                            time += Time.fixedDeltaTime;
                            var intensity = Mathf.PerlinNoise(Time.time * speed, 0.0f);
                            Light.ChangeIntensity(OrigIntensity * intensity);
                            yield return yielder;
                        }
                        break;

                    case 1:
                        while (time <= duration)
                        {
                            var offDuration = (float)rand.NextDouble() * 0.5f;
                            var onDuration = (float)rand.NextDouble() * 0.5f;

                            Light.SetEnabled(false);
                            yield return new WaitForSeconds(offDuration);
                            time += offDuration;

                            Light.SetEnabled(true);
                            yield return new WaitForSeconds(onDuration);
                            time += onDuration;
                        }
                        break;
                }
                
            }
        }

        private void StopAnimation()
        {
            if (LightAnimationRoutine != null)
            {
                OwnerZone.StopCoroutine(LightAnimationRoutine);
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
                        OwnerZone = zone,
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

        public void SetLightSetting(WEE_ZoneLightData data)
        {
            if (Replicator == null)
                return;

            if (Replicator.IsInvalid)
                return;

            var seed = data.Seed;
            if (seed == 0)
            {
                seed = new System.Random().Next();
            }

            Replicator.SetState(new ZoneLightState()
            {
                lightData = data.LightDataID,
                lightSeed = seed,
                duration = data.TransitionDuration
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
                    LightsInZone[i].ApplyLightSetting(block, isRecall ? 0.0f:state.duration, state.lightSeed, i);
                }
            }
        }
    }
}
