using BepInEx.IL2CPP.Utils.Collections;
using ExtendedWardenEvents.Jsons.Il2CppJson;
using ExtendedWardenEvents.WEE.Converter;
using ExtendedWardenEvents.WEE.Detours;
using ExtendedWardenEvents.WEE.Events;
using GameData;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtendedWardenEvents.WEE
{
    public static class WardenEventExt
    {
        internal readonly static Dictionary<WEEType, BaseEvent> _EventsToTrigger = new();

        static WardenEventExt()
        {
            var eventTypes = typeof(BaseEvent).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsAssignableTo(typeof(BaseEvent)));

            foreach (var type in eventTypes)
            {
                var instance = (BaseEvent)Activator.CreateInstance(type);
                if (_EventsToTrigger.TryGetValue(instance.EventType, out var existing))
                {
                    Logger.Error($"Duplicate {nameof(BaseEvent.EventType)} Detected!");
                    Logger.Error($"With '{existing.Name}' and '{instance.Name}'");
                    continue;
                }
                _EventsToTrigger[instance.EventType] = instance;
            }
        }

        internal static void Initialize()
        {
            Il2CppJsonConverters.RegisterConverter(new EventDataConverter());
            WEEEnumInjector.Inject();
            Detour_ExecuteEvent.Patch();
        }

        internal static void HandleEvent(WEEType type, WardenObjectiveEventData e, float currentDuration)
        {
            if (WEEDataHolder.TryGetWEEData(e, out var data))
            {
                CoroutineManager.StartCoroutine(Handle(type, data, currentDuration).WrapToIl2Cpp(), null);
            }
            else
            {
                Logger.Error($"WardenEvent Type is Extension ({type}) But it's not registered to dataholder!");
            }
        }

        private static IEnumerator Handle(WEEType type, WEE_EventData e, float currentDuration)
        {
            float delay = Mathf.Max(e.Delay - currentDuration, 0f);
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            WardenObjectiveManager.DisplayWardenIntel(e.Layer, e.WardenIntel);
            if (e.DialogueID > 0u)
            {
                PlayerDialogManager.WantToStartDialog(e.DialogueID, -1, false, false);
            }
            if (e.SoundID > 0u)
            {
                WardenObjectiveManager.Current.m_sound.Post(e.SoundID, true);
                var line = e.SoundSubtitle.ToString();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    GuiManager.PlayerLayer.ShowMultiLineSubtitle(line);
                }
            }

            if (e.SubObjective.DoUpdate)
            {
                WardenObjectiveManager.UpdateSyncCustomSubObjective(e.SubObjective.CustomSubObjectiveHeader, e.SubObjective.CustomSubObjective);
            }

            if (e.Fog.DoUpdate)
            {
                EnvironmentStateManager.AttemptStartFogTransition(e.Fog.FogSetting, e.Fog.FogTransitionDuration, e.DimensionIndex);
            }

            if (_EventsToTrigger.TryGetValue(type, out var eventInstance))
            {
                eventInstance.Trigger(e);
            }
            else
            {
                Logger.Error($"{type} does not exist in lookup!");
            }
            yield break;
        }
    }
}
