using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Modules.WOE;

internal abstract class WOE_ContextBase
{
    public abstract eWardenObjectiveType TargetType { get; }
}
