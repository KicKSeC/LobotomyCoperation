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
        protected override void OnInteracted(Pawn caster)
        { 
            CompUsable comp = parent.GetComp<CompUsable>();
            comp.TryStartUseJob(caster, comp.GetExtraTarget(caster));
        } 

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            yield break;
        }
    }

    public class CompProperties_ContainmentBox : CompProperties
    {
        public CompProperties_SpawnsAbnormality SpawnsAbnormality { get; set; }

        public CompProperties_ContainmentBox()
        {
            compClass = typeof(CompContainmentBox);
        }
    }
}
