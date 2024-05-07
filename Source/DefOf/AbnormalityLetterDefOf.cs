using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Abnormality
{
    [DefOf]
    public static class AbnormalityLetterDefOf
    {
        [MayRequireAnomaly]
        public static LetterDef AbnormalityDiscovered;

        static AbnormalityLetterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
        }
    }
}
