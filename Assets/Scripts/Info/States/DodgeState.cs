

namespace FireControl.Info
{
    /// <summary>
    /// 闪避状态案例
    /// </summary>
    public class DodgeState : CharacterState
    {
        public DodgeState()
        {
            this.stateID = 1;
            stateName = "闪避状态";
        }

        public override void EnterStateBehavior()
        {
            return;
        }

        public override void ExitStateBehavior()
        {
            return;
        }

        /// <summary>
        /// 闪避状态是无敌的，不会消耗血量，直接返回0
        /// </summary>
        public override int OnHPChange(int changeHP, CharacterInfo info)
        {
            return 0;
        }

        public override int OnSPChange(int changeSP)
        {
            return changeSP;
        }

    }
}