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
        public static ThingDef ContainmentBox;
        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimWorld.ThingDefOf));
        }
    }

}
