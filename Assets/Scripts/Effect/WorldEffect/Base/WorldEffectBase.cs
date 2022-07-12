
using UnityEngine;

namespace FireControl.Effect
{
    public abstract class WorldEffectBase 
    {
        /// <summary>
        /// ��������ֹͣʱ�ɵ����飬һ����Ҫ����UI
        /// </summary>
        public Common.Handler.HandlerList.INonReturnAndNonParam nonReturnAndNonParam;
        /// <summary>
        /// ��ʼ����ʱ���еķ���
        /// </summary>
        public abstract void OnBegin();
        /// <summary>
        /// ÿһ֡����ʱִ�е���Ϊ,����ֵ�����ж��Ƿ���ɣ������ʱ����true
        /// </summary>
        public abstract bool OnFixedUpdate();
        /// <summary>
        /// ����ʱִ�е���Ϊ
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