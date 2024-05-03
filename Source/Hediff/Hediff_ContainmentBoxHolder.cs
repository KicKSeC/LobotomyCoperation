using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class Hediff_ContainmentBoxHolder : HediffWithComps
    {
        private CompProperties_SpawnsAbnormality spawns;

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            spawns = pawn.kindDef.race.GetCompProperties<CompProperties_SpawnsAbnormality>();

            if (spawns == null)
            {
                Log.Error("Invaild value CompProperties_SpawnsAbnormality is null");
            }
        }

        // 이 hediff는 Abnormality가 죽었을 때 그 자신의 CompProperties_SpawnsAbnormality가 할당된 containmentbox를 생성한다. 
        public override void Notify_PawnKilled()
        {
            base.Notify_PawnKilled();

            if (pawn.SpawnedOrAnyParentSpawned && GenDrop.TryDropSpawn(ThingMaker.MakeThing(Assign_ContainmentBox()), pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near, out Thing containmentBox))
            {  
                // 메시지 출력
                string text = pawn.LabelShort;
                if (pawn.IsMutant)
                {
                    text = Verse.Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.mutant.Def.label);
                }
                else if (pawn.Name == null)
                {
                    text = Verse.Find.ActiveLanguageWorker.WithDefiniteArticle(text);
                }
                Messages.Message("MessageContainmentBoxDropped".Translate(text).CapitalizeFirst(), containmentBox, MessageTypeDefOf.NeutralEvent);
            }
            else
            {
                Log.Error("cannot generate ContainerBox");
            }
        }
        public ThingDef Assign_ContainmentBox()
        {
            // Containment Box에 CompProperties_SpawnsAbnormality 할당  
            ThingDef def = ThingDefOf.ContainmentBox;
            List<CompProperties> comps = new List<CompProperties>();
            bool isTargetFound = false; 
            foreach (var comp in def.comps)
            {
                if (comp is CompProperties_ContainmentBox box)
                {
                    isTargetFound = true;
                    box.SpawnsAbnormality = spawns;
                    comps.Add(box);
                }
                else
                {
                    comps.Add(comp);
                }
            }
            if (!isTargetFound)
            {
                Log.Error("comps does not have CompProperties_ContainmentBox");
            }

            def.comps = comps;
            return def;
        }
    }
}
