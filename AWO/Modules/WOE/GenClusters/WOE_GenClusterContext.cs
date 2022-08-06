using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Modules.WOE.GenClusters;

internal sealed class WOE_GenClusterContext : WOE_ContextBase
{
    public override eWardenObjectiveType TargetType => eWardenObjectiveType.CentralGeneratorCluster;
}
