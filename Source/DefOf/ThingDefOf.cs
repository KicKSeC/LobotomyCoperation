using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{ 
    [DefOf]
    public static class ThingDefOf
    {
        public static ThingDef BoxOneSin;
        public static ThingDef BoxNothingThere;
        public static ThingDef BoxSingingMachine;

        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimWorld.ThingDefOf));
        }
    }

}
