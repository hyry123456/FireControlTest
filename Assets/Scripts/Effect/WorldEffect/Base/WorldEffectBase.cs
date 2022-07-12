
using UnityEngine;

namespace FireControl.Effect
{
    public abstract class WorldEffectBase 
    {
        /// <summary>
        /// 用来听后停止时干的事情，一般需要开启UI
        /// </summary>
        public Common.Handler.HandlerList.INonReturnAndNonParam nonReturnAndNonParam;
        /// <summary>
        /// 开始动画时进行的方法
        /// </summary>
        public abstract void OnBegin();
        /// <summary>
        /// 每一帧调用时执行的行为,返回值用来判断是否完成，当完成时返回true
        /// </summary>
        public abstract bool OnFixedUpdate();
        /// <summary>
        /// 结束时执行的行为
        /// </summary>
        public virtual void OnEnd()
        {
            if(nonReturnAndNonParam != null)
            {
                nonReturnAndNonParam();
            }
        }
    }
}