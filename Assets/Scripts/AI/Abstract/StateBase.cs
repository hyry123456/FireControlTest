

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
        /// 进入状态机执行的方法
        /// </summary>
        /// <param name="stateMachine">状态机对象</param>
        /// <param name="enterFuntion">附加函数</param>
        virtual public void EnterState(FightAIStateMachine stateMachine, Common.Handler.HandlerList.INonReturnAndNonParam enterFuntion)
        {
            this.stateMachine = stateMachine;
            if (enterFuntion != null)
                enterFuntion();
        }

        /// <summary>
        /// 逐帧执行方法，用来逐帧检测
        /// </summary>
        /// <returns>返回下一个要进入的状态，不用切换状态就返回null</returns>
        abstract public StateBase OnFixedUpdate();

        /// <summary>
        /// 退出状态，加载下一个状态，然后返回下一个状态
        /// </summary>
        /// <returns>下一个状态,抽象父类的返回值没有意义</returns>
        virtual public void ExitState(Common.Handler.HandlerList.INonReturnAndNonParam exitFuntion)
        {
            if(exitFuntion != null)
                exitFuntion();
        }
    }
}