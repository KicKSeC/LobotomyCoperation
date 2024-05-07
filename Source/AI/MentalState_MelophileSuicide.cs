using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Abnormality.AI
{
    public class MentalState_MelophileSuicide : MentalState
    {
        // TODO: 어딘가에서 순환 에러가 발생한 듯 하여 게임이 강제 종료된다. 이걸 해결하라. 
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void PreStart()
        {
            base.PreStart(); 
            if (!IsReachableSingingMachine())
            {
                RecoverFromState();
            }
        }

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if(pawn.IsHashIntervalTick(120) && !IsReachableSingingMachine())
            {
                RecoverFromState();
            }
        }

        public bool IsReachableSingingMachine()
        {
            var singingMachines = pawn.Map.mapPawns.AllPawns.Where(thing => thing.HasComp<CompSingingMachine>());
            if (singingMachines.EnumerableNullOrEmpty())
            {
                return false;
            }

            bool reachable = false;
            foreach(var singingMachine in singingMachines)
            {
                if (pawn.CanReach(singingMachine, PathEndMode.Touch, Danger.None, canBashDoors: true))
                {
                    reachable = true; 
                    break;
                }
            }
            return reachable;
        }
    }
}
