using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Abnormality
{
    public class CompProperties_SpawnsNothingThere : CompProperties_SpawnsAbnormality
    {
        public CompProperties_SpawnsNothingThere() 
        {
            compClass = typeof(CompSpawnsNothingThere);
        }
    }
}
