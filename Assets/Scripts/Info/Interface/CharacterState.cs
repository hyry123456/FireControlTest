

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
        /// <summary>        /// ����״̬��Ϊ        /// </summary>
        public abstract void EnterStateBehavior();
        /// <summary>        /// �˳�״̬��Ϊ        /// </summary>
        public abstract void ExitStateBehavior();

        /// <summary>        
        /// ��HP�ı�ʱִ�У�����һЩ����֮���Buff�������˺�ʲô��,
        /// ����Ҫ�ı������ֱ�ӷ��ش���ֵ����
        /// </summary>
        /// <param name="changeHP">�ı��HPֵ</param>
        /// <returns>���ظı��ĸı�ֵ</returns>
        public abstract int OnHPChange(int changeHP, CharacterInfo info);

        /// <summary>
        /// ��SP�ı�ʱִ�У�����һЩװ����Buff������Ҫʱֱ�ӷ��ش���ֵ����
        /// </summary>
        /// <param name="changeSP">�ı�SP��С</param>
        /// <returns>�ı���SPֵ</returns>
        public abstract int OnSPChange(int changeSP);
    }
}