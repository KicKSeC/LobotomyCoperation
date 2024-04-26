using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace LobotomyCoperation
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
            if (spawnTick > 0 && Find.TickManager.TicksGame > spawnTick && parent.MapHeld != null)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(PawnKindAbnormalityDefOf.NothingThere, Faction.OfEntities);
                GenSpawn.Spawn(pawn, parent.PositionHeld, parent.MapHeld);
                CompNothingThere compNothingThere = pawn.TryGetComp<CompNothingThere>(); 
                Find.LetterStack.ReceiveLetter("LetterLabelNothingThereEmergence".Translate(), "LetterNothingThereEmergence".Translate(), LetterDefOf.ThreatBig, pawn);
                parent.Destroy();
            }
        }
    }
}
