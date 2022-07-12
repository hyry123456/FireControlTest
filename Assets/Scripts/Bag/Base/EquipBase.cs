

namespace FireControl.Equip
{
    /// <summary>
    /// 这个类是一个装备的基类，一个装备的基本属性就是都定义在这个类中的。
    /// 需要注意的是我这个设定的装备是有数量限制的，也就是说每一个装备有且仅有一件
    /// 如果需要指定数量，可以使用装备耐久代替
    /// </summary>
    public abstract class EquipBase
    {
        public int equipID;
        public EquipType equipType;
        /// <summary>        /// 装备介绍文本        /// </summary>
        public string equipIntroduce;
        /// <summary>        /// 装备名称        /// </summary>
        public string equipName;
        /// <summary>        /// 用来作为反射创建对象时的根据字符串        /// </summary>
        public string className;

        /// <summary>
        /// 初始化方法，记得初始化装备的ID，装备类型，装备介绍文本
        /// 还有类名称，装备名称
        /// </summary>
        protected abstract void Initialize();
    }
}

