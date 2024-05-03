using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Abnormality.Comp
{
    public class CompAbnormality : ThingComp
    {
        protected Pawn Abnormality => (Pawn)parent;
        [Unsaved(false)]
        protected Hediff_ContainmentBoxHolder containmentBoxHolder;  

        public override void CompTick()
        {
            base.CompTick();
            
            if (Abnormality.health == null)
            {
                Log.Error("Abnormality.health is null");
            }

            if (containmentBoxHolder == null) 
            { 
                containmentBoxHolder = InitHediff();
            }
        }

        private Hediff_ContainmentBoxHolder InitHediff()
        {
            try
            {
                Hediff hediff = Abnormality.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.ContainmentBoxHolder) ?? Abnormality.health.AddHediff(HediffDefOf.ContainmentBoxHolder);
                return (Hediff_ContainmentBoxHolder)hediff;
            } 
            catch (Exception ex)
            {
                Log.Error($"cannot add hediff with excetion {ex}"); 
                return null;
            }
        }
    }
}
