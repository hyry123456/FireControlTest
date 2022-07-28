using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// 运行方法的基类，方便方法重载
    /// </summary>
    public abstract class FireNodeBase : MonoBehaviour
    {
        /// <summary>        
        /// 已经燃烧的火焰值，范围是0-100
        /// </summary>
        public float fireValue = 0;
        /// <summary>        /// 燃烧范围，用来确定可以影响到的物体        /// </summary>
        public float burnRange = 1;
        [Range(0, 0.1f)]
        /// <summary>        /// 易燃性        /// </summary>
        public float inflammability = 0.05f;
        /// <summary>        /// 火焰强度，也就是火焰的上涨速度，范围是0-1     /// </summary>
        public float fireIntensity;

        public float FireIntensity
        {
            get { return fireIntensity; }
        }

        protected virtual void Start()
        {
            //之后可以常考使用文本加载
            fireValue = Random.Range(0f, 20f);
            fireIntensity = Random.Range(0.0f, 0.5f);
        }

        /// <summary>
        /// 进行燃烧周围物体以及自身的方法，进行对可影响物体的燃烧以及自身的燃烧，
        /// 该方法并不是逐帧调，运算量大，因此建议只进行燃烧赋值
        /// </summary>
        /// <param name="fireMap">所有燃烧物体的存储结构体</param>
        /// <param name="index">该物体的编号</param>
        public abstract void RotinuFuction(FireMap fireMap, int index);

        /// <summary>
        /// 自己燃烧的方法，该方法是逐帧调用的，负责更新自己的燃烧情况以及一些逐帧的特效,
        /// 基类实现了火焰的自燃控制
        /// </summary>
        public virtual void BurnUpdate()
        {
            float fire01 = fireValue / 100.0f;
            float change = Mathf.Cos(fire01 * Mathf.PI * 0.65f);
            fireIntensity = Mathf.Clamp01(fireIntensity + change * Time.deltaTime * inflammability * fireIntensity);
            fireValue += fireIntensity * Time.deltaTime;
        }

        public virtual void AddFireIntensity(float addSize)
        {
            fireIntensity += addSize * inflammability ;
            fireIntensity = Mathf.Clamp01(fireIntensity);
        }

    }
}