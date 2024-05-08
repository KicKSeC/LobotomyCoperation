using Abnormality;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompNothingThere : CompAbnormality
    {  
        public override ThingDef GetContainmentBox()
        {
            return ThingDefOf.BoxNothingThere;
        }

        public override void OnActivityActivated()
        {
            if (Pawn.MapHeld == null)
            {
                return;
            }
            if (Pawn.IsOnHoldingPlatform)
            {
                var building_HoldingPlatform = (Building_HoldingPlatform)Pawn.ParentHolder;
                building_HoldingPlatform.innerContainer.TryDrop(Pawn, building_HoldingPlatform.Position, building_HoldingPlatform.Map, ThingPlaceMode.Direct, 1, out var _);
                CompHoldingPlatformTarget compHoldingPlatformTarget = Pawn.TryGetComp<CompHoldingPlatformTarget>();
                if (compHoldingPlatformTarget != null)
                {
                    compHoldingPlatformTarget.targetHolder = null;
                }
            }
        }

        public override void OnPassive()
        {
            throw new NotImplementedException();
        }
    } 

    public class CompProperties_NothingThere : CompProperties_Abnormality
    {
        public CompProperties_NothingThere() 
        { 
            compClass = typeof(CompNothingThere);
        }
    }
}
