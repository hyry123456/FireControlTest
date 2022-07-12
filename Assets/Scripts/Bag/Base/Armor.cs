

namespace FireControl.Equip
{
    /// <summary>
    /// 防具类，具体实现和武器类很像，不过一个是防御类型的，我们可能之后有火焰的烧伤效果之类的，需要使用防具的防御事件进行防护，也就是受伤时会调用这个防具事件，不过一般都只是简单的减少伤害而已，不是用复杂的buff增加。
    ///  </summary>

    public abstract class Armor : EquipBase
    {
        public int defenseValue;
        public ArmorType armorType;
        /// <summary>        /// 装备耐久，也就是使用次数        /// </summary>
        public int equipNegor;
        /// <summary>        /// 最大耐久值        /// </summary>
        public int maxNegogor;
        public abstract EquipReturn Defense();
    }
}

