using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class ChoiceLetter_AbnormalityDiscovered : ChoiceLetter
    {
        public AbnormalityCodexEntryDef codexEntry;

        public override IEnumerable<DiaOption> Choices
        {
            get
            {
                if (codexEntry != null)
                {
                    foreach (ResearchProjectDef project in codexEntry.discoveredResearchProjects)
                    {
                        yield return new DiaOption("ViewHyperlink".Translate(project.label))
                        {
                            action = delegate
                            {
                                Verse.Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research);
                                if (MainButtonDefOf.Research.TabWindow is MainTabWindow_Research mainTabWindow_Research)
                                {
                                    mainTabWindow_Research.Select(project);
                                }
                            },
                            resolveTree = true
                        };
                    }

                    yield return Option_OpenAbnormalityCodex;
                }

                yield return base.Option_Close;
            }
        }

        protected DiaOption Option_OpenAbnormalityCodex => new DiaOption("ViewAbnormalityCodex".Translate())
        {
            action = delegate
            {
                if (codexEntry != null)
                {
                    Verse.Find.WindowStack.Add(new Dialog_AbnormalityCodex(codexEntry));
                }
            },
            resolveTree = true
        };

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref codexEntry, "codexEntry");
        }
    }
}
