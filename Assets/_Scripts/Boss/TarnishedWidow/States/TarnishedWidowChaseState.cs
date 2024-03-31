using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies.SharedStates;

namespace DarkHavoc.Boss.TarnishedWidow.States
{
    public class TarnishedWidowChaseState : BossChaseState
    {
        public bool RangedAvailable { get; private set; }
        public bool MeleeAvailable { get; private set; }
        public bool BuffAvailable { get; private set; }
        public bool JumpAvailable { get; private set; }

        public TarnishedWidowChaseState(DarkHavoc.Boss.TarnishedWidow.TarnishedWidow boss, float stoppingDistance) : base(boss, stoppingDistance)
        {
        }

        public override void FixedTick()
        {
            base.FixedTick();
            MeleeAvailable = ((DarkHavoc.Boss.TarnishedWidow.TarnishedWidow)boss).MeleeHitBox.IsPlayerInRange();
            BuffAvailable = ((DarkHavoc.Boss.TarnishedWidow.TarnishedWidow)boss).BuffHitBox.IsPlayerInRange();
            RangedAvailable = ((DarkHavoc.Boss.TarnishedWidow.TarnishedWidow)boss).RangedHitBox.IsPlayerInRange();
            JumpAvailable = ((DarkHavoc.Boss.TarnishedWidow.TarnishedWidow)boss).JumpHitBox.IsPlayerInRange();
        }
        
        public override void OnExit()
        {
            BuffAvailable = false;
            RangedAvailable = false;
            MeleeAvailable = false;
            base.OnExit();
        }
    }
}