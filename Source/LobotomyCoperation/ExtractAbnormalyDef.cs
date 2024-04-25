using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI.Group;
using Verse;

namespace LobotomyCoperation
{
    public class ExtractAbnormalyDef : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve psychicShockChanceFromQualityCurve;

        public FloatRange darkPsychicShockDurarionHoursRange;

        public FloatRange incidentDelayHoursRange;

        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new ExtractAbnormaly(InvokerRole, psychicShockChanceFromQualityCurve));
            return list;
        }

        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(psychicShockChanceFromQualityCurve.Evaluate(qualityRange.min).ToStringPercent());
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }
        }
    }
}
