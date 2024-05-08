using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Abnormality
{
    public abstract class CompAbnormality : CompInteractable, IActivity
    {
        public bool IsUnstable = true;
        public Pawn Pawn => (Pawn)parent;
        public new CompProperties_Abnormality Props => (CompProperties_Abnormality)props;
        public CompActivity Activity => activityComp ?? (activityComp = Pawn.GetComp<CompActivity>());

        private CompActivity activityComp;
        public override void CompTick()
        {
            base.CompTick();   
        }

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo); 

            if (GenDrop.TryDropSpawn(ThingMaker.MakeThing(GetContainmentBox()), parent.PositionHeld, prevMap, ThingPlaceMode.Near, out Thing Box))
            {
                // 메시지 출력
                string text = parent.LabelShort;  
                Messages.Message("MessageContainmentBoxDropped".Translate(text).CapitalizeFirst(), Box, MessageTypeDefOf.NeutralEvent);
            }
            else
            {
                Log.Error("cannot generate ContainerBox which is null");
            }
        }

        public abstract ThingDef GetContainmentBox();
        public bool CanBeSuppressed() => !IsUnstable;
        public bool CanActivate() => true;
        public string ActivityTooltipExtra() => Props.unstableInspectText.Colorize(ColoredText.WarningColor);
        public bool ShouldGoPassive() => false;
        public abstract void OnActivityActivated();
        public abstract void OnPassive();
    }

    public abstract class CompProperties_SpawnsAbnormality : CompProperties
    {

    }

    public abstract class CompProperties_Abnormality : CompProperties_Interactable 
    {
        [MustTranslate]
        public string unstableInspectText;


        public CompProperties_Abnormality()
        {
            compClass = typeof(CompAbnormality);
        }
    }
}
