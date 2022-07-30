using LevelGeneration;
using Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.WEE.Events.World
{
    internal sealed class LockSecurityDoorEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.LockSecurityDoor;

        protected override void TriggerCommon(WEE_EventData e)
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

            var locks = door.m_locks.TryCast<LG_SecurityDoor_Locks>();
            if (locks != null)
            {
                var text = door.LinkedToZoneData?.ProgressionPuzzleToEnter?.CustomText?.ToText() ?? string.Empty;
                locks.m_intCustomMessage.m_message = text;
            }

            if (IsMaster)
            {
                var state = door.m_sync.GetCurrentSyncState();
                switch (state.status)
                {
                    case eDoorStatus.Closed:
                    case eDoorStatus.Closed_LockedWithBulkheadDC:
                    case eDoorStatus.Closed_LockedWithChainedPuzzle:
                    case eDoorStatus.Closed_LockedWithChainedPuzzle_Alarm:
                    case eDoorStatus.Closed_LockedWithKeyItem:
                    case eDoorStatus.Closed_LockedWithNoKey:
                    case eDoorStatus.Closed_LockedWithPowerGenerator:
                    case eDoorStatus.Unlocked:
                        door.m_sync.AttemptDoorInteraction(eDoorInteractionType.SetLockedNoKey, 0.0f, 0.0f, default, null);
                        break;
                }
            }
        }
    }
}
