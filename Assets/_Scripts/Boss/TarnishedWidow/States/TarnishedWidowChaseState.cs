using DarkHavoc.Boss.SharedStates;

namespace DarkHavoc.Boss.TarnishedWidow.States
{
    public class TarnishedWidowChaseState : BossChaseState
    {
        public bool RangedAvailable { get; private set; }
        public bool MeleeAvailable { get; private set; }
        public bool BuffAvailable { get; private set; }
        public bool JumpAvailable { get; private set; }

        public TarnishedWidowChaseState(TarnishedWidow boss, float stoppingDistance) : base(boss, stoppingDistance)
        {
        }

        public override void FixedTick()
        {
            base.FixedTick();
            MeleeAvailable = ((TarnishedWidow)boss).MeleeHitBox.IsPlayerInRange();
            BuffAvailable = ((TarnishedWidow)boss).BuffHitBox.IsPlayerInRange();
            RangedAvailable = ((TarnishedWidow)boss).RangedHitBox.IsPlayerInRange();
            JumpAvailable = ((TarnishedWidow)boss).JumpHitBox.IsPlayerInRange();
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