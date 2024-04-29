using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompProperties_Extractor : CompProperties
    {
        public CompProperties_Extractor() 
        {
            compClass = typeof(CompExtractor);
        }
    }
}
