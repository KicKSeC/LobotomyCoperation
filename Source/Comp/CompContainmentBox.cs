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
        public CompProperties_SpawnsAbnormality abnormalitySpawnsAssigned;
        
        protected override void OnInteracted(Pawn caster)
        { 
            CompUsable comp = parent.GetComp<CompUsable>();
            comp.TryStartUseJob(caster, comp.GetExtraTarget(caster));
        }
        
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            if(
                abnormalitySpawnsAssigned == null && 
                Find.CompProperties_SpawnsAbnormalitiesDict.Values.TryRandomElement(out var result)
            ) {
                abnormalitySpawnsAssigned = result;
            }
            else
            {
                Log.Error("Cannot assign abnormality in containment box");
            }
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            yield break;
        }
    }
}
