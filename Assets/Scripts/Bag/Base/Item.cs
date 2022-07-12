

namespace FireControl.Equip
{
    public abstract class Item : EquipBase
    {
        /// <summary>     /// 物件数量    /// </summary>
        public int count;

        /// <summary>
        /// 存储物件的效果，对于一些装备使用时可以有一些效果
        /// 但是有一些没有，只是一个物品，所以使用该属性判断是否可以使用
        /// </summary>
        private bool isCanUse;

        /// <summary>    /// 获取这个物体是否是可使用物体的根据属性   /// </summary>
        public bool IsCanUse
        {
            get { return isCanUse; }
        }

        public abstract EquipReturn ItemUseFunction();
    }
}