using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWO.Modules.WOE;
internal abstract class WOE_DataBase
{
    public uint ObjectiveID { get; set; }
    public WardenObjectiveDataBlock GameData { get; set; }
}
