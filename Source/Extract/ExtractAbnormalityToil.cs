using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace Abnormality
{
    public class ExtractAbnormalityToil : PsychicRitualToil
    {  
        public PsychicRitualRoleDef targetRole;

        public PsychicRitualRoleDef invokerRole;

        public FloatRange extractionQuilityRange;

        protected ExtractAbnormalityToil()
        {
        }

        public ExtractAbnormalityToil(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, FloatRange extractionQuilityRange)
        {
            this.invokerRole = invokerRole;
            this.targetRole = targetRole;
            this.extractionQuilityRange = extractionQuilityRange;
        }

        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(targetRole); 
            if (pawn != null && pawn2 != null)
            {
                ApplyOutcome(psychicRitual, pawn, pawn2);
            }
        }

        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
        {
            IntVec3 cell = psychicRitual.assignments.Target.Cell; 
            CompContainmentBox compBox = new CompContainmentBox(); 
            
            // assign abnormality
            psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(target.PositionHeld, psychicRitual.Map), target.PositionHeld, 60);
            SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(target.PositionHeld, target.Map));  

            psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(cell, psychicRitual.Map), cell, 60);
            SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, psychicRitual.Map));
            target.Destroy(); 
            Thing box = ThingMaker.MakeThing(GetRandomContainmentBox());
            box.TryGetComp<CompContainmentBox>(out compBox);

            TaggedString text = "ExtractAbnormalityCompleteText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL"), target.Named("TARGET"));
            Verse.Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), text, LetterDefOf.NeutralEvent, new LookTargets(box));
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
        }

        private ThingDef GetRandomContainmentBox()
        {
            return Find.ContainmentBoxes.RandomElement();
        }
    }
}
