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

        public override void OnActivityActivated()
        {
            throw new NotImplementedException();
        }

        public override void OnPassive()
        {
            throw new NotImplementedException();
        }
    }

    public class CompProperties_OneSin: CompProperties_Abnormality
    {
        public CompProperties_OneSin()
        {
            compClass = typeof(CompOneSin);
        }
    }
}
