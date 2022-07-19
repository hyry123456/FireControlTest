using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// ���з����Ļ��࣬���㷽������
    /// </summary>
    public abstract class FireRoutineBase : MonoBehaviour
    {
        /// <summary>        /// ����ֵȼ�չ���ֵ        /// </summary>
        public float fireValue = 0;

        protected virtual void Start()
        {
            fireValue = 0;
        }

        /// <summary>
        /// ��������ȼ���õķ�������������Χ���������
        /// </summary>
        /// <param name="nodeCount">ȼ�յ�����������ȷ�����ѭ��һ��</param>
        public abstract void RotinuFuction(int nodeCount);

        /// <summary>
        /// �������ӻ���ֵ�ķ�������ͬ�����������ӵķ�ʽ���ܲ�һ���������Ҫ��������
        /// ȼ�ճ̶������㷨�������Ƕ���һЩ�Զ�������
        /// </summary>
        /// <param name="fireValue">Ȼ�󴫵�Դ�Ļ����С(0-1)</param>
        /// <param name="distance">����Դ��Ӱ��̶ȴ�С(0-1)</param>
        public abstract void AddBurn(float fireValue, float distance);

    }
}