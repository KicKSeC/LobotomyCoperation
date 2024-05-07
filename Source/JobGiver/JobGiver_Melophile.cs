using Abnormality.AI;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Abnormality
{
    public class JobGiver_Melophile : ThinkNode_JobGiver
    { 
        protected override Job TryGiveJob(Pawn pawn)
        {
            if(!(pawn.MentalState is MentalState_Melophile melophile))
            {
                return null;
            }

            Pawn target = melophile.target;
            var singingMachine = pawn.Map.mapPawns.AllPawns.Where(pawn1 => pawn1.HasComp<CompSingingMachine>());
            if (singingMachine.EnumerableNullOrEmpty())
            { 
                return null;
            }

            Job job = JobMaker.MakeJob(RimWorld.JobDefOf.InteractThing, target, singingMachine.RandomElement()); 
            job.count = 1;
            return job; 
        }
    }
}
