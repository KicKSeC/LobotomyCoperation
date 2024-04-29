using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompNothingThere : ThingComp
    {
        public CompProperties_SpawnsAbnormality spawns;
        public override void PostPostMake()
        {
            if (Find.CompProperties_SpawnsAbnormalitiesDict.TryGetValue(Find.Abnormality.NothingThere, out var compProperties_SpawnsAbnormality))
            {
                spawns = compProperties_SpawnsAbnormality;
            }
            else
            {
                Log.Error("cannot assign nothing there spawns");
            }
        }
    }
}
