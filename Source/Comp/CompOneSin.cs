using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompOneSin : CompAbnormality
    {
        public override ThingDef GetContainmentBox()
        {
            return ThingDefOf.BoxOneSin;
        }
    }
}
