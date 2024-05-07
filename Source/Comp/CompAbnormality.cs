using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Abnormality
{
    public abstract class CompAbnormality : ThingComp
    { 
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
    }

    public abstract class CompProperties_SpawnsAbnormality : CompProperties
    {

    }
}
