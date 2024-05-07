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
    public static class MentalStateDefOf
    {
        public static MentalStateDef Melophile;
        public static MentalStateDef MelophileKiller;
        public static MentalStateDef MelophileSuicide;

        static MentalStateDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
        }
    }
}
