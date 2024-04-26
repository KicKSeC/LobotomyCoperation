using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace LobotomyCoperation
{
    public class CompProperties_NothingThere : CompProperties
    {
        public CompProperties_NothingThere() 
        { 
            compClass = typeof(CompNothingThere);
        }
    }
}
