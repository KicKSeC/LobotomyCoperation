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
    public class MentalState_MelophileKiller : MentalState
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
                TrySetMelophile();
            }
        }

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (target.Dead)
            { 
                if (!TrySetMelophile())
                {
                    Log.Error("cannot change mental state into melophile");
                }
            }
            if (pawn.IsHashIntervalTick(120) && !IsTargetStillValidAndReachable())
            {
                if (!TryFindNewTarget())
                {
                    if (TrySetMelophile())
                    {
                        Log.Error("cannot change mental state into melophile");
                    }
                    return;
                }
                Messages.Message("MessageMelophileKillerChangedTarget".Translate(pawn.NameShortColored, target.Label, pawn.Named("PAWN"), target.Named("TARGET")).Resolve().AdjustedFor(pawn), pawn, MessageTypeDefOf.NegativeEvent);
                base.MentalStateTick();
            }
        }

        public override TaggedString GetBeginLetterText()
        {
            if (target == null)
            {
                Log.Error("No target. This should have been checked in this mental state's worker.");
                return "";
            }
            return def.beginLetter.Formatted(pawn.NameShortColored, target.NameShortColored, pawn.Named("PAWN"), target.Named("TARGET")).AdjustedFor(pawn).Resolve()
                .CapitalizeFirst();
        }

        private bool TryFindNewTarget()
        {
            target = MurderousRageMentalStateUtility.FindPawnToKill(pawn);
            return target != null;
        }

        private bool TrySetMelophile()
        { 
            return pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Melophile, forced: true, forceWake: false, causedByMood: false, transitionSilently: true);
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
