
namespace FireControl.Equip
{
    /// <summary>
    /// 武器类型枚举，用来确定该装备类型，使用枚举是因为以后可能会有其他类型的装备。
    /// </summary>

    public enum EquipType
    {
        /// <summary>        /// 武器类型的装备        /// </summary>
        Weapon = 0,
        /// <summary>        /// 防具类型的装备        /// </summary>
        Armor = 1,
        /// <summary>    /// 小物品，这个是数量类型，不是按照耐久区分了   /// </summary>
        Item = 2,
    }
}
