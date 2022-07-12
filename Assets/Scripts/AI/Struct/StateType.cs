
namespace FireControl.AI
{
    public enum StateType
    {
        /// <summary>        /// 默认状态，怪物在挂机或者巡逻之类的        /// </summary>
        Default,
        /// <summary>    ///  查看状态机，看目标是否在可以看见，可以看见就进入攻击，否则回到默认     /// </summary>
        Check,
        /// <summary>        /// 攻击状态        /// </summary>
        Attack
    }
}