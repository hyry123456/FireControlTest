using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// 运行方法的基类，方便方法重载
    /// </summary>
    public abstract class FireRoutineBase : MonoBehaviour
    {
        /// <summary>        /// 火焰值燃烧过的值        /// </summary>
        public float fireValue = 0;

        protected virtual void Start()
        {
            fireValue = 0;
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

    }
}