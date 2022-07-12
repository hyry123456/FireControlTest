
namespace FireControl.Interaction
{
    /// <summary>
    /// 交互类型，注意，交互类型实际上是
    /// </summary>
    public enum InteractionType
    {
        Object = 1,
        PasserBy = 2,
        Enemy = 4,
        Task = 8,
        /// <summary>       /// 运动交互        /// </summary>
        Move = 16,
    }
}