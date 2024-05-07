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

        public Pawn target; 

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref target, "target");
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void PreStart()
        {
            base.PreStart();
            if (!TryFindNewTarget())
            {
                TrySetMelophileKiller();
            }
        }

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (pawn.IsHashIntervalTick(120) && !IsTargetStillValidAndReachable())
            { 
                if (!TryFindNewTarget())
                { 
                    if (pawn.Map.mapPawns.AllHumanlike.Empty())
                    {
                        TrySetMelophileSuicide();
                    }
                    if (!TrySetMelophileKiller())
                    { 
                        Log.Error("cannot change mental state");
                    }
                    return;
                }
                Messages.Message("MessageMelophileChangedTarget".Translate(pawn.NameShortColored, target.Label, pawn.Named("PAWN"), target.Named("TARGET")).Resolve().AdjustedFor(pawn), pawn, MessageTypeDefOf.NegativeEvent);
                base.MentalStateTick();
            }
        }

        public override TaggedString GetBeginLetterText()
        {
            // def가 null이래요 
            return def.beginLetter.Formatted(pawn.NameShortColored, target.NameShortColored, pawn.Named("PAWN"), target.Named("TARGET")).AdjustedFor(pawn).Resolve()
                .CapitalizeFirst();
        }

        private bool TryFindNewTarget()
        {
            bool Validator(Thing t)
            {
                Pawn body = (Pawn)t;
                return body.Dead && body.RaceProps.Humanlike && pawn.CanReach(body, PathEndMode.None, Danger.Deadly);
            }

            target = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn), validator: Validator);
            return target != null;
        }

        private bool TrySetMelophileKiller()
        {
            return pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.MelophileKiller, forced: true, forceWake: false, causedByMood: false);
        }

        private bool TrySetMelophileSuicide()
        {
            return pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.MelophileSuicide, forced: true, forceWake: false, causedByMood: false);
        }

        public bool IsTargetStillValidAndReachable()
        {
            if (target != null && target.SpawnedParentOrMe != null && (!(target.SpawnedParentOrMe is Pawn) || target.SpawnedParentOrMe == target))
            {
                return pawn.CanReach(target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, canBashDoors: true);
            }
            return false;
        } 
    }
}
