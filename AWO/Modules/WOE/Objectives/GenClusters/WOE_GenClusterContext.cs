using System;

namespace AWO.Modules.WOE.Objectives.GenClusters;

internal sealed class WOE_GenClusterContext : WOE_ContextBase
{
    public override eWardenObjectiveType TargetType => eWardenObjectiveType.CentralGeneratorCluster;

    public override Type DataType => typeof(int);
}
