using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompContainmentBox : CompInteractable
    {  
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            yield break;
        }
    }

    public class CompProperties_ContainmentBox : CompProperties
    {
        public CompProperties_ContainmentBox()
        {
            compClass = typeof(CompContainmentBox);
        }
    }
}
