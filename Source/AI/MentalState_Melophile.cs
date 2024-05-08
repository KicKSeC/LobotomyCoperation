using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Abnormality.AI
{
    public class MentalState_Melophile : MentalState
    {

        public Corpse corpse; 

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref corpse, "corpse"); 
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void PreStart()
        {
            base.PreStart();
            TryFindNewTarget();
        }

        // TODO: 일단 시체를 들고 있을 땐 탐색에서 제외시켜야겠다. corpse obssesion을 참고해서 해봐야지. 
        public override void MentalStateTick()
        {
            base.MentalStateTick();

            if(IsHaulingCorpse())
            { 
                return;
            }

            if (pawn.IsHashIntervalTick(120) && !IsTargetStillValidAndReachable())
            { 
                if (!TryFindNewTarget())
                { 
                    if (pawn.Map.mapPawns.ColonistsSpawnedCount < 2)
                    {
                        TrySetMelophileSuicide();
                    }
                    else if (!TrySetMelophileKiller())
                    { 
                        Log.Error("cannot change mental state");
                    }
                    return;
                }
                Messages.Message("MessageMelophileChangedTarget".Translate(pawn.NameShortColored, corpse.Label, pawn.Named("PAWN"), corpse.Named("TARGET")).Resolve().AdjustedFor(pawn), pawn, MessageTypeDefOf.NeutralEvent);
                base.MentalStateTick();
            }
        }

        public bool IsTargetStillValidAndReachable()
        {
            if (corpse != null && corpse.SpawnedParentOrMe != null && (!(corpse.SpawnedParentOrMe is Pawn) || corpse.SpawnedParentOrMe == corpse))
            {
                return pawn.CanReach(corpse.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, canBashDoors: true);
            }
            return false;
        }

        public bool IsHaulingCorpse()
        {
            if(pawn.CurJob != null && pawn.CurJob.def == RimWorld.JobDefOf.InteractThing)
            {
                Thing thing = pawn.CurJob.targetB.Thing;
                if (thing != null && thing is Corpse corpse)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryFindNewTarget()
        {
            bool Validator(Thing thing)
            {
                return thing is Corpse corpse && corpse.InnerPawn.RaceProps.Humanlike && !corpse.Destroyed && pawn.CanReach(corpse, PathEndMode.None, Danger.Deadly);
            }

            corpse = (Corpse)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Corpse), PathEndMode.Touch, TraverseParms.For(pawn), validator: Validator);
            return corpse != null;
        }

        private bool TrySetMelophileKiller()
        {
            return pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.MelophileKiller, forced: true, forceWake: false, causedByMood: false);
        }

        private bool TrySetMelophileSuicide()
        {
            return pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.MelophileSuicide, forced: true, forceWake: false, causedByMood: false);
        }

    }
}
