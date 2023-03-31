using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Modules.WOE.Uplinks;
internal sealed class WOE_UplinkData : WOE_DataBase
{
    public bool ShowCodesOnTerminal { get; set; } = false;
    public bool ShowCodesOnHUD { get; set; } = true;
}
