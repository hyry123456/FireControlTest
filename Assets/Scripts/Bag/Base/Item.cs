

namespace FireControl.Equip
{
    public abstract class Item : EquipBase
    {
        /// <summary>     /// �������    /// </summary>
        public int count;

        /// <summary>
        /// �洢�����Ч��������һЩװ��ʹ��ʱ������һЩЧ��
        /// ������һЩû�У�ֻ��һ����Ʒ������ʹ�ø������ж��Ƿ����ʹ��
        /// </summary>
        private bool isCanUse;

        /// <summary>    /// ��ȡ��������Ƿ��ǿ�ʹ������ĸ�������   /// </summary>
        public bool IsCanUse
        {
            get { return isCanUse; }
        }

        public abstract EquipReturn ItemUseFunction();
    }
}