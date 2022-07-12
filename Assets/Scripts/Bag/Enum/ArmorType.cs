
namespace FireControl.Equip
{
    /// <summary>
    /// 该枚举列举了所有类型的防具，我们之后创建的所有类型的防具都需要定义这个属性，确定该防具类型，好确定这个防具应该放到背包的哪个位置。
    /// </summary>

    public enum ArmorType
    {
        /// <summary>        /// 头盔        /// </summary>
        Helmet,
        /// <summary>        /// 夹克/上衣        /// </summary>
        Jacket,
        /// <summary>        /// 裤子        /// </summary>
        Trouser,
        /// <summary>        /// 手套        /// </summary>
        Glove,
        /// <summary>        /// 鞋子        /// </summary>
        Shoe
    }
}

