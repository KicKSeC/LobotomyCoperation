using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Abnormality
{ 
    public static class Find
    {
        public static AbnormalityCodex AbnormalityCodex = new AbnormalityCodex();

        public static List<ThingDef> ContainmentBoxes = new List<ThingDef> { 
            ThingDefOf.BoxOneSin, ThingDefOf.BoxNothingThere, ThingDefOf.BoxSingingMachine
        };
    }
}
