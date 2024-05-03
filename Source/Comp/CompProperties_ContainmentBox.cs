using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompProperties_ContainmentBox : CompProperties
    { 
        public CompProperties_SpawnsAbnormality SpawnsAbnormality { get; set; }

        public CompProperties_ContainmentBox() 
        {
            compClass = typeof(CompContainmentBox);
        }
    }
}
