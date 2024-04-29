using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Abnormality
{
    public class CompExtractor : ThingComp
    {  
        public Lord GetLord()
        {
            if (parent is Building b)
            {
                return b.GetLord();
            }
            if (parent is Pawn p)
            {
                return p.GetLord();
            }
            if (parent is Corpse c)
            {
                return c.GetLord();
            }
            return null;
        }

        public PsychicRitual GetPsychicRitual()
        {
            return (GetLord()?.CurLordToil as LordToil_PsychicRitual)?.RitualData.psychicRitual;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad); 
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder(base.CompInspectStringExtra()); 
            Lord lord;
            if ((lord = GetLord()) != null && lord.CurLordToil is LordToil_PsychicRitual lordToil_PsychicRitual)
            {
                PsychicRitual psychicRitual = lordToil_PsychicRitual.RitualData.psychicRitual;
                PsychicRitualToil psychicRitualToil = lordToil_PsychicRitual.RitualData.psychicRitualToil;
                stringBuilder.AppendInNewLine(string.Format("{0}: {1}", "PsychicRitual".Translate().CapitalizeFirst(), psychicRitual.def.LabelCap));
                stringBuilder.AppendInNewLine(psychicRitualToil.GetReport(psychicRitual, null).CapitalizeFirst().EndWithPeriod());
            }
            return stringBuilder.ToString();
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            base.PostDrawExtraSelectionOverlays();
            if (parent.Faction != Faction.OfPlayer)
            {
                return;
            } 
        }

        public override void CompTick()
        {
            base.CompTick(); 
            PsychicRitual psychicRitual;
            if ((psychicRitual = GetPsychicRitual()) != null && parent.Faction == Faction.OfPlayer)
            {
                psychicRitual.CancelPsychicRitual("PsychicRitualAreaObstructed".Translate());
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            PsychicRitual psychicRitual;
            if ((psychicRitual = GetPsychicRitual()) != null)
            {
                yield return PsychicRitualGizmo.CancelGizmo(psychicRitual);
            }
            else
            {
                foreach (Gizmo gizmo in PsychicRitualGizmo.GetGizmos(parent))
                { 
                    yield return gizmo;
                }
            }
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Reset psychic ritual cooldowns",
                    action = delegate
                    {
                        Verse.Find.PsychicRitualManager.ClearAllCooldowns();
                    }
                };
            }
        }
    }
}
