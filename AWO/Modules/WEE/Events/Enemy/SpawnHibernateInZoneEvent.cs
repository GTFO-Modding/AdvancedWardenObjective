using UnityEngine;
using AWO.WEE.Events;
using GTFO.API.Utilities;
using System.Collections;
using LevelGeneration;
using Agents;
using Enemies;
using AIGraph;

namespace AWO.Modules.WEE.Events.Enemy;
internal class SpawnHibernateInZoneEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.SpawnHibernateInZone;

    private const float TimeToCompleteSpawn = 2.0f;

    protected override void TriggerMaster(WEE_EventData e) 
    {
        if (!TryGetZone(e, out var zone) || zone == null) return;

        var sh = e.SpawnHibernates;

        if (sh.Count == 1 && sh.Position != Vector3.zero) // spawn 1 enemies on specific position
        {
            if (!IsValidAreaIndex(sh.AreaIndex, zone)) return;

            EnemyAllocator.Current.SpawnEnemy(sh.EnemyID, zone.m_areas[sh.AreaIndex].m_courseNode, AgentMode.Hibernate, sh.Position, Quaternion.Euler(sh.Rotation));
        }
        else 
        {
            if(sh.AreaIndex != -1)
            {
                if (!IsValidAreaIndex(sh.AreaIndex, zone)) return;
            }

            CoroutineDispatcher.StartCoroutine(DoSpawn(sh, zone));
        }
    }

    static IEnumerator DoSpawn(WEE_SpawnHibernateData e, LG_Zone zone)
    {
        // TODO: how to fking spawn scout conveniently?
        AgentMode mode = AgentMode.Hibernate;
        float SpawnInterval = TimeToCompleteSpawn / e.Count;

        var rand = new System.Random();
        var areas = zone.m_areas;

        for (int SpawnCount = 0; SpawnCount < e.Count; SpawnCount++)
        {
            AIG_CourseNode spawnNode = e.AreaIndex != -1 ? areas[e.AreaIndex].m_courseNode : areas[rand.Next(0, areas.Count)].m_courseNode;
            // scout:
            Quaternion rotation = Quaternion.LookRotation(new Vector3(EnemyGroup.s_randomRot2D.x, 0.0f, EnemyGroup.s_randomRot2D.y), Vector3.up);
            EnemyAllocator.Current.SpawnEnemy(e.EnemyID, spawnNode, mode,
                spawnNode.GetRandomPositionInside()/* TODO: improve position random - enemies will spawn at the same position by low chances */,
                rotation);

            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    private static bool IsValidAreaIndex(int areaIndex, LG_Zone zone)
    {
        var areas = zone.m_areas;
        if(areaIndex < 0 || areaIndex >= areas.Count)
        {
            Logger.Error($"Invalid area index {areaIndex}");
            return false;
        }

        return true;
    }
}
