

namespace FireControl.Info
{
    /// <summary>
    /// ����״̬����
    /// </summary>
    public class DodgeState : CharacterState
    {
        public DodgeState()
        {
            this.stateID = 1;
            stateName = "����״̬";
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
        /// ����״̬���޵еģ���������Ѫ����ֱ�ӷ���0
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