using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class AbnormalityCodexEntryDef : EntityCodexEntryDef
    { 
        public new AbnormalityCategoryDef category;

        public new bool Discovered => Find.AbnormalityCodex.Discovered(this);
    }
}
