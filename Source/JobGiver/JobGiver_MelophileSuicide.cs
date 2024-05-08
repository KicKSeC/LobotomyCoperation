using Abnormality.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Abnormality
{
    public class JobGiver_MelophileSuicide : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        { 
            if (!(pawn.MentalState is MentalState_MelophileSuicide melophileSuicide))
            {
                return null;
            }
            var singingMachines = pawn.Map.mapPawns.AllPawns.Where(pawn1 => pawn1.HasComp<CompSingingMachine>());

            if (singingMachines.EnumerableNullOrEmpty())
            { 
                return null; 
            }

            Job job = JobMaker.MakeJob(RimWorld.JobDefOf.InteractThing, singingMachines.RandomElement(), pawn);
            job.count = 1;
            return job;
        }
    }
}
