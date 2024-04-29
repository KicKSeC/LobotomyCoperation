using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompSpawnsNothingThere : ThingComp
    {
        public int spawnTick = -1;

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref spawnTick, "spawnTick", 0);
        }

        public override void CompTickRare()
        {
            if (spawnTick > 0 && Verse.Find.TickManager.TicksGame > spawnTick && parent.MapHeld != null)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(PawnKindAbnormalityDefOf.NothingThere, Faction.OfEntities);
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.ContainmentBoxHolder, pawn);
                pawn.health.AddHediff(hediff);
                GenSpawn.Spawn(pawn, parent.PositionHeld, parent.MapHeld); 
                Verse.Find.LetterStack.ReceiveLetter("LetterLabelNothingThereEmergence".Translate(), "LetterNothingThereEmergence".Translate(), LetterDefOf.ThreatBig, pawn);
                parent.Destroy();
            }
        }
    }
}
