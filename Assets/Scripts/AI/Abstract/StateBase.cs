

namespace FireControl.AI
{
    public abstract class StateBase 
    {
        protected FightAIStateMachine stateMachine;
        protected StateType stateType;
        public StateType StateType
        {
            get { return stateType; }
        }

        /// <summary>
        /// ����״̬��ִ�еķ���
        /// </summary>
        /// <param name="stateMachine">״̬������</param>
        /// <param name="enterFuntion">���Ӻ���</param>
        virtual public void EnterState(FightAIStateMachine stateMachine, Common.Handler.HandlerList.INonReturnAndNonParam enterFuntion)
        {
            this.stateMachine = stateMachine;
            if (enterFuntion != null)
                enterFuntion();
        }

        /// <summary>
        /// ��ִ֡�з�����������֡���
        /// </summary>
        /// <returns>������һ��Ҫ�����״̬�������л�״̬�ͷ���null</returns>
        abstract public StateBase OnFixedUpdate();

        /// <summary>
        /// �˳�״̬��������һ��״̬��Ȼ�󷵻���һ��״̬
        /// </summary>
        /// <returns>��һ��״̬,������ķ���ֵû������</returns>
        virtual public void ExitState(Common.Handler.HandlerList.INonReturnAndNonParam exitFuntion)
        {
            if(exitFuntion != null)
                exitFuntion();
        }
    }
}