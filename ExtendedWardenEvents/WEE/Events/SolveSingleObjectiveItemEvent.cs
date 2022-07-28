namespace ExtendedWardenEvents.WEE.Events
{
    internal sealed class SolveSingleObjectiveItemEvent : BaseEvent
    {
        public override WEEType EventType => WEEType.SolveSingleObjectiveItem;

        protected override void TriggerMaster(WEE_EventData e)
        {
            if (!WardenObjectiveManager.HasWardenObjectiveDataForLayer(e.Layer))
            {
                LogError($"{e.Layer} Objective is Missing");
                return;
            }

            var chainIndex = WardenObjectiveManager.GetCurrentChainIndex(e.Layer);
            var items = WardenObjectiveManager.GetObjectiveItemCollection(e.Layer, chainIndex);
            if (items == null)
            {
                LogError($"{e.Layer} Objective Doesn't have ObjectiveItem Collection!");
                return;
            }

            foreach (var item in items)
            {
                if (item.ObjectiveItemSolved)
                    continue;

                WardenObjectiveManager.OnLocalPlayerSolvedObjectiveItem(e.Layer, item, forceSolve: false);
                break;
            }
        }
    }
}
