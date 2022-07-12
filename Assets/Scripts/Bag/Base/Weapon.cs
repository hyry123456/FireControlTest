
using UnityEngine;

namespace FireControl.Equip
{
    /// <summary>
    /// 武器基类，所有武器类都要继承自该类，因为我们之后调用武器的事件时一般也就是调用这个函数而已，这样也方便修改
    /// 同时需要注意的是，建议武器类不继承Monobehavior，所以在构造函数时要初始化好武器类型，不然之后进行匹配时出错就麻烦了
    /// </summary>

    public abstract class Weapon : EquipBase
    {
        public int attackValue;
        /// <summary>        /// 创建的坐标，像特效，伤害的距离判断都是以该点为根据的
        /// 该属性不是赋值上去的，而是有Weapon启动时在文件中加载(加载后给主角)，然后自动读取的/// </summary>
        public Transform createPos;
        /// <summary>        /// 装备耐久，也就是使用次数        /// </summary>
        public int equipNeogor;
        /// <summary>        /// 最大耐久值        /// </summary>
        public int maxNeogor;

        public abstract EquipReturn WeaponBehavior(Bag bag);
    }
}