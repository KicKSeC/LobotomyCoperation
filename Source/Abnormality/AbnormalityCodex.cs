using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class AbnormalityCodex : IExposable
    {
        private Dictionary<AbnormalityCategoryDef, bool> hiddenCategories;

        private Dictionary<AbnormalityCodexEntryDef, bool> hiddenEntries;

        private HashSet<ThingDef> discoveredAbnormalities;

        public bool debug_UnhideAllResearch;

        public const string AbnormalityDiscoveredSignal = "AbnormalityDiscovered";

        private const int AbnormalityDiscoveredLetterDelayTicks = 600;

        private const int MonolithInteractableLetterDelayTicks = 900;

        public AbnormalityCodex()
        {
            hiddenCategories = new Dictionary<AbnormalityCategoryDef, bool>();
            hiddenEntries = new Dictionary<AbnormalityCodexEntryDef, bool>();
            discoveredAbnormalities = new HashSet<ThingDef>();
            foreach (AbnormalityCategoryDef allDef in DefDatabase<AbnormalityCategoryDef>.AllDefs)
            {
                hiddenCategories.Add(allDef, value: true);
            }
        }

        public static int EntryCountInCategory(AbnormalityCategoryDef def)
        {
            int num = 0;
            foreach (AbnormalityCodexEntryDef allDef in DefDatabase<AbnormalityCodexEntryDef>.AllDefs)
            {
                if (allDef.category == def)
                {
                    num++;
                }
            }
            return num;
        }

        public int DiscoveredCount(AbnormalityCategoryDef def)
        {
            int num = 0;
            foreach (AbnormalityCodexEntryDef allDef in DefDatabase<AbnormalityCodexEntryDef>.AllDefs)
            {
                if (allDef.category == def && Find.AbnormalityCodex.Discovered(allDef))
                {
                    num++;
                }
            }
            return num;
        }

        public bool Hidden(ResearchProjectDef def)
        {
            if (!ModsConfig.AnomalyActive)
            {
                return false;
            }
            if (debug_UnhideAllResearch)
            {
                return false;
            }
            if (def.knowledgeCategory != null && Verse.Find.Anomaly.Level < 1)
            {
                return true;
            }
            foreach (AbnormalityCodexEntryDef allDef in DefDatabase<AbnormalityCodexEntryDef>.AllDefs)
            {
                if (allDef.discoveredResearchProjects.NotNullAndContains(def))
                {
                    return !Discovered(allDef);
                }
            }
            return false;
        }

        public bool Discovered(AbnormalityCategoryDef def)
        {
            if (!ModLister.CheckAnomaly("Abnormality codex"))
            {
                return false;
            }
            if (hiddenCategories.TryGetValue(def, out var value))
            {
                return !value;
            }
            return false;
        }

        public bool Discovered(AbnormalityCodexEntryDef entry)
        {
            if (!ModLister.CheckAnomaly("Abnormality codex"))
            {
                return false;
            }
            if (!Discovered(entry.category))
            {
                return false;
            }
            if (hiddenEntries.TryGetValue(entry, out var value))
            {
                return !value;
            }
            return false;
        }

        public void SetDiscovered(List<AbnormalityCodexEntryDef> entries, ThingDef discoveredDef = null, Thing discoveredThing = null)
        {
            foreach (AbnormalityCodexEntryDef entry in entries)
            {
                SetDiscovered(entry, discoveredDef, discoveredThing);
            }
            Verse.Find.HiddenItemsManager.SetDiscovered(discoveredDef);
        }

        public void SetDiscovered(AbnormalityCodexEntryDef entry, ThingDef discoveredDef = null, Thing discoveredThing = null)
        {
            if (!ModLister.CheckAnomaly("Abnormality codex"))
            {
                return;
            }
            if (discoveredDef != null)
            {
                discoveredAbnormalities.Add(discoveredDef);
            }
            if (Discovered(entry))
            {
                return;
            }
            hiddenEntries[entry] = false;
            hiddenCategories[entry.category] = false;
            Verse.Find.SignalManager.SendSignal(new Signal("AbnormalityDiscovered", global: true));
            if (Current.ProgramState != ProgramState.Playing || Verse.Find.Anomaly.Level <= 0)
            {
                return;
            }
            if (discoveredThing != null)
            {
                Messages.Message(string.Format("{0}: {1}", "MessageAbnormalityDiscovered".Translate(), entry.LabelCap), discoveredThing, MessageTypeDefOf.PositiveEvent);
            }
            if (!entry.discoveredResearchProjects.NullOrEmpty())
            {
                string text = string.Empty;
                if (!entry.discoveredResearchProjects.NullOrEmpty())
                {
                    string arg = discoveredThing?.LabelShort ?? discoveredDef?.label ?? entry.label;
                    text += "LetterTextAbnormalityDiscoveryResearch".Translate(arg.Named("ABNORMALITY")) + ":\n" + ((IEnumerable<ResearchProjectDef>)entry.discoveredResearchProjects).Select((Func<ResearchProjectDef, string>)((ResearchProjectDef r) => r.LabelCap)).ToLineList("  - ");
                }
                if (!text.NullOrEmpty())
                { 
                    ChoiceLetter_AbnormalityDiscovered choiceLetter_AbnormalityDiscovered = (ChoiceLetter_AbnormalityDiscovered)LetterMaker.MakeLetter("LetterLabelAbnormalityDiscovery".Translate(), text, LetterDefOf.EntityDiscovered); 
                    choiceLetter_AbnormalityDiscovered.codexEntry = entry;
                    if (discoveredThing != null)
                    {
                        choiceLetter_AbnormalityDiscovered.lookTargets = discoveredThing;
                    }
                    Verse.Find.LetterStack.ReceiveLetter(choiceLetter_AbnormalityDiscovered, null, 600);
                }
            }
            MonolithLevelDef levelDef = Verse.Find.Anomaly.LevelDef;
            if (levelDef.level > Verse.Find.Anomaly.lastLevelActivationLetterSent && Verse.Find.Anomaly.monolith.CanActivate(out var _, out var _) && !levelDef.activatableLetterLabel.NullOrEmpty() && !levelDef.activatableLetterText.NullOrEmpty())
            {
                Verse.Find.Anomaly.lastLevelActivationLetterSent = levelDef.level;
                Verse.Find.LetterStack.ReceiveLetter(levelDef.activatableLetterLabel, levelDef.activatableLetterText, LetterDefOf.PositiveEvent, Verse.Find.Anomaly.monolith, null, null, null, null, 900);
            }
        }

        public bool Discovered(ThingDef def)
        {
            if (ModLister.CheckAnomaly("Abnormality codex"))
            {
                return discoveredAbnormalities.Contains(def);
            }
            return false;
        }

        public void Debug_DiscoverAll()
        {
            foreach (AbnormalityCodexEntryDef allDef in DefDatabase<AbnormalityCodexEntryDef>.AllDefs)
            {
                SetDiscovered(allDef);
            } 
            foreach (AbnormalityCategoryDef allDef3 in DefDatabase<AbnormalityCategoryDef>.AllDefs)
            {
                hiddenCategories[allDef3] = false;
            }
        }

        public void Reset()
        {
            hiddenCategories.Clear();
            hiddenEntries.Clear();
            discoveredAbnormalities.Clear();
            foreach (AbnormalityCategoryDef allDef in DefDatabase<AbnormalityCategoryDef>.AllDefs)
            {
                hiddenCategories.Add(allDef, value: true);
            }
            foreach (AbnormalityCodexEntryDef allDef2 in DefDatabase<AbnormalityCodexEntryDef>.AllDefs)
            {
                hiddenEntries.Add(allDef2, !allDef2.startDiscovered);
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref hiddenCategories, "hiddenCategories", LookMode.Def, LookMode.Value);
            Scribe_Collections.Look(ref hiddenEntries, "hiddenEntries", LookMode.Def, LookMode.Value);
            Scribe_Collections.Look(ref discoveredAbnormalities, "discoveredEntities", LookMode.Def);
            Scribe_Values.Look(ref debug_UnhideAllResearch, "debug_UnhideAllResearch", defaultValue: false);
            if (Scribe.mode != LoadSaveMode.PostLoadInit)
            {
                return;
            }
            if (discoveredAbnormalities == null)
            {
                discoveredAbnormalities = new HashSet<ThingDef>();
            }
            Dictionary<AbnormalityCategoryDef, bool> dictionary = new Dictionary<AbnormalityCategoryDef, bool>();
            foreach (AbnormalityCategoryDef allDef in DefDatabase<AbnormalityCategoryDef>.AllDefs)
            {
                dictionary.Add(allDef, value: true);
            }
            foreach (KeyValuePair<AbnormalityCategoryDef, bool> hiddenCategory in hiddenCategories)
            {
                if (dictionary.ContainsKey(hiddenCategory.Key))
                {
                    dictionary[hiddenCategory.Key] = hiddenCategory.Value;
                }
            }
            hiddenCategories = dictionary;
            Dictionary<AbnormalityCodexEntryDef, bool> dictionary2 = new Dictionary<AbnormalityCodexEntryDef, bool>();
            foreach (AbnormalityCodexEntryDef allDef2 in DefDatabase<AbnormalityCodexEntryDef>.AllDefs)
            {
                dictionary2.Add(allDef2, !allDef2.startDiscovered);
            }
            foreach (KeyValuePair<AbnormalityCodexEntryDef, bool> hiddenEntry in hiddenEntries)
            {
                if (dictionary2.ContainsKey(hiddenEntry.Key))
                {
                    dictionary2[hiddenEntry.Key] = hiddenEntry.Value;
                }
            }
            hiddenEntries = dictionary2;
            foreach (ThingDef discoveredAbnormality in discoveredAbnormalities)
            {
                Verse.Find.HiddenItemsManager.SetDiscovered(discoveredAbnormality);
            }
        }
    }
}
