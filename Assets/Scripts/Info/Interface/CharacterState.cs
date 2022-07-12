

namespace FireControl.Info
{
    public abstract class CharacterState
    {
        protected int stateID;
        protected string stateName;
        public string StateName
        {
            get { return stateName; }
        }
        new public bool Equals(object obj)
        {
            if((obj as CharacterState).stateID == this.stateID)
                return true;
            else return false;
        }
        /// <summary>        /// 进入状态行为        /// </summary>
        public abstract void EnterStateBehavior();
        /// <summary>        /// 退出状态行为        /// </summary>
        public abstract void ExitStateBehavior();

        /// <summary>        
        /// 在HP改变时执行，用于一些防具之类的Buff，减少伤害什么的,
        /// 不需要改变的数据直接返回传入值即可
        /// </summary>
        /// <param name="changeHP">改变的HP值</param>
        /// <returns>返回改变后的改变值</returns>
        public abstract int OnHPChange(int changeHP, CharacterInfo info);

        /// <summary>
        /// 在SP改变时执行，用于一些装备的Buff，不需要时直接返回传入值即可
        /// </summary>
        /// <param name="changeSP">改变SP大小</param>
        /// <returns>改变后的SP值</returns>
        public abstract int OnSPChange(int changeSP);
    }
}