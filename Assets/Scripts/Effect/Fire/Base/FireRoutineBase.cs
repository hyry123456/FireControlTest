using UnityEngine;

namespace FireControl.Effect.Fire
{
    /// <summary>
    /// 运行方法的基类，方便方法重载
    /// </summary>
    public abstract class FireRoutineBase : MonoBehaviour
    {
        /// <summary>        /// 当前火焰值        /// </summary>
        public float fireValue = 0;
        /// <summary>        /// 耐烧程度，也就是燃烧时扩散的速度        /// </summary>
        [Range(0,1.0f)]
        public float spreadSpeed = 0.0001f;
        /// <summary>        /// 易燃速度，决定火焰燃烧提升速度        /// </summary>
        public float fireAddSpeed = 0.5f;
        /// <summary>        /// 当前燃烧过的范围        /// </summary>
        public float burnRange = 0;

        protected virtual void Start()
        {
            fireValue = 0;
            burnRange = 0;
        }

        /// <summary>
        /// 用于自身燃烧用的方法，在烧完周围物体后会调用
        /// </summary>
        /// <param name="nodeCount">燃烧的数量，用于确定多久循环一次</param>
        public abstract void RotinuFuction(int nodeCount);

        /// <summary>
        /// 用于增加火焰值的方法，不同类型物体增加的方式可能不一样，因此需要独立设置
        /// 燃烧程度增加算法，尤其是定义一些自定义设置
        /// </summary>
        /// <param name="fireValue">然后传递源的火焰大小(0-1)</param>
        /// <param name="distance">传递源的影响程度大小(0-1)</param>
        public abstract void AddBurn(float fireValue, float distance);

        /// <summary>
        /// 开始燃烧方法，让这个物体开始燃烧，燃烧值为随机值
        /// </summary>
        public virtual void BeginFire()
        {
            fireValue = Random.Range(0.0f, 1.0f);
        }
    }
}