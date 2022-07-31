using AIGraph;
using GameData;
using LevelGeneration;

namespace ExtendedWardenEvents.WEE.Events.SecDoor
{
    internal sealed class CloseSecurityDoorEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.CloseSecurityDoor;

        protected override void TriggerMaster(WEE_EventData e)
        {
            if (!TryGetZone(e, out var zone))
            {
                LogError("Cannot find zone!");
                return;
            }

            if (!TryGetZoneEntranceSecDoor(zone, out var door))
            {
                LogError("Cannot find Security Door!");
                return;
            }

            var state = door.m_sync.GetCurrentSyncState();
            if (state.status == eDoorStatus.Open || state.status == eDoorStatus.Opening)
            {
                Logger.Debug("Door Closed!");

                var sync = door.m_sync.TryCast<LG_Door_Sync>();
                if (sync == null)
                    return;

                var syncState = sync.GetCurrentSyncState();
                syncState.status = eDoorStatus.Closed;
                syncState.hasBeenOpenedDuringGame = false;
                sync.m_stateReplicator.State = syncState;

                var gate = door.Gate;
                gate.HasBeenOpenedDuringPlay = false;
                gate.IsTraversable = false;

                var nodeDistanceFrom = gate.m_linksFrom.m_courseNode.m_playerCoverage.GetNodeDistanceToPlayer();
                var nodeDistanceBehind = gate.m_linksTo.m_courseNode.m_playerCoverage.GetNodeDistanceToPlayer();
                AIG_CourseNode clearNode;
                if (nodeDistanceFrom < nodeDistanceBehind)
                {
                    clearNode = gate.m_linksTo.m_courseNode;
                }
                else
                {
                    clearNode = gate.m_linksFrom.m_courseNode;
                }

                Logger.Debug("Despawning Enemies Behind Securiy Door...");
                AIG_SearchID.IncrementSearchID();
                DespawnEnemiesInNearNodes(AIG_SearchID.SearchID, clearNode);
            }
        }

        private void DespawnEnemiesInNearNodes(ushort searchID, AIG_CourseNode sourceNode)
        {
            if (sourceNode == null)
                return;

            if (sourceNode.m_portals == null)
                return;

            foreach (var enemy in sourceNode.m_enemiesInNode.ToArray())
            {
                enemy.m_replicator.Despawn();
            }

            foreach (var portal in sourceNode.m_portals)
            {
                if (portal == null)
                    continue;

                if (portal.m_searchID == searchID)
                    continue;

                portal.m_searchID = searchID;
                if (portal.IsProgressionLocked)
                    continue;

                var behindNode = portal.GetOppositeNode(sourceNode);
                if (behindNode != null)
                {
                    DespawnEnemiesInNearNodes(searchID, behindNode);
                }
            }
        }
    }
}
