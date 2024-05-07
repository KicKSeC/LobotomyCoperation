using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Abnormality
{
    public class CompSingingMachine : CompAbnormality
    {
        public override ThingDef GetContainmentBox()
        {
            return ThingDefOf.BoxSingingMachine;
        }

        // TODO: 죽은 타겟의 시체를 갈갈해서 랜덤 림에게 Melophile을 부여하는 상호작용 
    }
}
