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
    // TODO: singing machine이 죽을 때 박스 두 개를 뱉는다. 하나만 뱉게 해야 한다. 
    public class CompSingingMachine : CompAbnormality
    {
        public override ThingDef GetContainmentBox()
        { 
            return ThingDefOf.BoxSingingMachine;
        }

        public override void OnActivityActivated()
        {
            if (Pawn.MapHeld == null)
            {
                return;
            }

            MakeRandomPawnGetMelophile();
            Activity.EnterActiveState(); 
        }

        public override void OnPassive()
        {
            return;
        }

        // 상호작용할 림을 선택할 때 화면에 UI가 지워지고 아무것도 못하게 되는 문제
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach(Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
        }

        public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
        {
            AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
            if(!result.Accepted)
            {
                return result;
            } 
            return true;
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {   
            AcceptanceReport acceptance = CanInteract(selPawn);
            FloatMenuOption floatMenuOption = new FloatMenuOption(Props.jobString.CapitalizeFirst(), delegate
            {
                OrderActivate(selPawn);
            });
            if (!acceptance.Accepted)
            {
                floatMenuOption.Disabled = true;
                floatMenuOption.Label = floatMenuOption.Label + " (" + acceptance.Reason + ")";
            }
            yield return floatMenuOption;
        }
         
        protected override void OnInteracted(Pawn caster)
        {
            OnActivityActivated();
            Messages.Message("CommandActivate".Translate(Pawn.Named("PAWN")), Pawn, MessageTypeDefOf.NeutralEvent);
        } 

        public void MakeRandomPawnGetMelophile()
        {
            var pawns = Pawn.MapHeld.mapPawns.AllHumanlike.Where(pawn => (!pawn.Inhumanized() && !pawn.health.Downed && pawn.IsColonist));
            pawns.RandomElement().mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Melophile, forced: true, forceWake: false, causedByMood: false);
        }

        private void OrderActivate(Pawn caster)
        {
            bool Validator(Thing thing)
            { 
                return thing is Corpse corpse && corpse.InnerPawn.RaceProps.Humanlike && !corpse.Destroyed && caster.CanReach(corpse, PathEndMode.None, Danger.Deadly);
            }

            Verse.Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("SingingMachineActivationConfirm".Translate(), delegate
            {
                Pawn corpse = (Pawn)GenClosest.ClosestThingReachable(caster.Position, caster.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(caster), validator: Validator);
                if (corpse != null) 
                {
                    Job job = JobMaker.MakeJob(RimWorld.JobDefOf.InteractThing, parent, corpse);
                    job.count = 1;
                    job.playerForced = true;
                    caster.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
                else
                {
                    Messages.Message("NoDeadPawnAvailable".Translate(), MessageTypeDefOf.NeutralEvent);
                }
            }));
        }
    }

    public class CompProperties_SingingMachine : CompProperties_Abnormality
    {
        [MustTranslate]
        public new string unstableInspectText;

        public CompProperties_SingingMachine()
        {
            compClass = typeof(CompSingingMachine);
        }
    }

    public class SingingMachineActivityWorker : PawnActivityWorker
    {  
        public override float GetChangeRatePerDay(ThingWithComps thing)
        {
            CompSingingMachine comp = thing.GetComp<CompSingingMachine>();
            float num = base.GetChangeRatePerDay(thing);
            if (comp.IsUnstable)
            {
                num += -0.05f;
            }
            return num;
        }

        public override void GetSummary(ThingWithComps thing, StringBuilder sb)
        {
            CompSingingMachine comp = thing.GetComp<CompSingingMachine>();
            base.GetSummary(thing, sb);
            if (comp.IsUnstable)
            {
                sb.Append(string.Format("\n - {0}: {1}", "Unstable".Translate(), (-0.05f).ToStringPercent("0")));
            }
        }
    }
}
