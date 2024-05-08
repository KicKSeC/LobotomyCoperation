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
    public class JobGiver_MelophileKiller : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        { 
            if (!(pawn.MentalState is MentalState_MelophileKiller melophileKiller) || !melophileKiller.IsTargetStillValidAndReachable())
            {
                return null;
            }
            Thing spawnedParentOrMe = melophileKiller.target.SpawnedParentOrMe;
            Job job = JobMaker.MakeJob(RimWorld.JobDefOf.AttackMelee, spawnedParentOrMe);
            job.count = 1;
            job.canBashDoors = true;
            job.killIncappedTarget = true;
            if (spawnedParentOrMe != melophileKiller.target)
            {
                job.maxNumMeleeAttacks = 1;
            }
            return job;
        }
    }
}
