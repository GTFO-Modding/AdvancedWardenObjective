﻿using UnityEngine;
using AWO.WEE.Events;
using GTFO.API.Utilities;
using System.Collections;
using LevelGeneration;
using Agents;
using Enemies;
using GameData;
using AIGraph;

namespace AWO.Modules.WEE.Events.Enemy;
internal class SpawnScoutInZoneEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.SpawnScoutInZone;

    private const float TimeToCompleteSpawn = 2.0f;

    protected override void TriggerMaster(WEE_EventData e) 
    {
        if (!TryGetZone(e, out var zone) || zone == null) return;

        var ss = e.SpawnScouts;
        if (ss.AreaIndex != -1)
        {
            if (ss.AreaIndex < 0 || ss.AreaIndex >= zone.m_areas.Count)
            {
                Logger.Error($"Invalid AreaIndex {ss.AreaIndex} for ZONE_{zone.Alias}");
                return;
            }
        }

        CoroutineDispatcher.StartCoroutine(DoSpawn(e, zone));
    }

    static IEnumerator DoSpawn(WEE_EventData e, LG_Zone zone)
    {
        var ss = e.SpawnScouts;
        System.Random rand = ss.AreaIndex == -1 ? new() : null;
        
        float SpawnInterval = TimeToCompleteSpawn / ss.Count;

        for (int SpawnCount = 0; SpawnCount < ss.Count; SpawnCount++)
        {
            EnemyGroupRandomizer r = null;
            if (!EnemySpawnManager.TryCreateEnemyGroupRandomizer(ss.GroupType, ss.Difficulty, out r) || r == null)
            {
                Logger.Error($"Invalid scout group: (GroupType: {ss.GroupType}, Difficulty: {ss.Difficulty})");
                yield break;
            }

            EnemyGroupDataBlock randomGroup = r.GetRandomGroup(Builder.SessionSeedRandom.Value());
            float popPoints = randomGroup.MaxScore * Builder.SessionSeedRandom.Range(1f, 1.2f);

            var node = ss.AreaIndex == -1 ? zone.m_areas[rand.Next(0, zone.m_areas.Count)].m_courseNode : zone.m_areas[ss.AreaIndex].m_courseNode;

            var scoutSpawnData = EnemyGroup.GetSpawnData(node.GetRandomPositionInside(), node, EnemyGroupType.Hibernating,
            eEnemyGroupSpawnType.RandomInArea, randomGroup.persistentID, popPoints) with
            {
                respawn = false
            };

            EnemyGroup.Spawn(scoutSpawnData);

            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
