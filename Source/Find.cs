using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Abnormality
{
    public static class Find
    {
        public static AbnormalityCodex AbnormalityCodex = new AbnormalityCodex();

        public enum Abnormality { NothingThere }
        public static Dictionary<Abnormality, CompProperties_SpawnsAbnormality> CompProperties_SpawnsAbnormalitiesDict = 
            new Dictionary<Abnormality, CompProperties_SpawnsAbnormality> {
                {Abnormality.NothingThere, new CompProperties_SpawnsNothingThere()}
        };
    }
}
