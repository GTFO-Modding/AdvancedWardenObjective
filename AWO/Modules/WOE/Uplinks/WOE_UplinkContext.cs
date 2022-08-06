using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Modules.WOE.Uplinks;

internal sealed class WOE_UplinkContext : WOE_ContextBase
{
    public override eWardenObjectiveType TargetType => eWardenObjectiveType.TerminalUplink;
}
