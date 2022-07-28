using GameData;
using LevelGeneration;

namespace ExtendedWardenEvents.WEE.Events
{
    internal sealed class TriggerSecurityDoorAlarmEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.TriggerSecurityDoorAlarm;

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

            door.UseChainedPuzzleOrUnlock(LocalPlayer.Owner);
            LogDebug($"{zone.NavInfo.GetFormattedText(LG_NavInfoFormat.Full_And_Number_No_Formatting)} Alarm Triggered!");
        }
    }
}
