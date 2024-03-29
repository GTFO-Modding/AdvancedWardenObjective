﻿using Agents;
using AWO.Modules.WEE;

namespace AWO.WEE.Events.Enemy;

internal sealed class AlertEnemiesInZoneEvent : BaseEvent
{
    public override WEE_Type EventType => WEE_Type.AlertEnemiesInZone;

    protected override void TriggerMaster(WEE_EventData e)
    {
        if (!TryGetZone(e, out var zone))
        {
            LogError("Zone is Missing?");
            return;
        }

        foreach (var node in zone.m_courseNodes)
        {
            if (node.m_enemiesInNode == null)
                continue;

            foreach (var enemy in node.m_enemiesInNode)
            {
                var mode = enemy.AI.Mode;
                if (mode == AgentMode.Hibernate)
                {
                    if (enemy.CourseNode.m_playerCoverage.GetNodeDistanceToClosestPlayer_Unblocked() > 2)
                    {
                        enemy.AI.m_behaviour.ChangeState(Enemies.EB_States.InCombat);
                    }
                    else
                    {
                        var delta = (LocalPlayer.Position - enemy.Position);
                        enemy.Locomotion.HibernateWakeup.ActivateState(delta.normalized, delta.magnitude, 0.0f, false);
                    }
                }
                else if (mode == AgentMode.Scout)
                {
                    enemy.Locomotion.ScoutScream.ActivateState(enemy.AI.m_behaviourData.GetTarget(LocalPlayer));
                }
            }
        }
    }
}
