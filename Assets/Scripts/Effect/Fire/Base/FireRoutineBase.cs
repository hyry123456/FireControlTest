using UnityEngine;

namespace FireControl.Effect.Fire
{
    /// <summary>
    /// ���з����Ļ��࣬���㷽������
    /// </summary>
    public abstract class FireRoutineBase : MonoBehaviour
    {
        /// <summary>        /// ��ǰ����ֵ        /// </summary>
        public float fireValue = 0;
        /// <summary>        /// ���ճ̶ȣ�Ҳ����ȼ��ʱ��ɢ���ٶ�        /// </summary>
        [Range(0,1.0f)]
        public float spreadSpeed = 0.0001f;
        /// <summary>        /// ��ȼ�ٶȣ���������ȼ�������ٶ�        /// </summary>
        public float fireAddSpeed = 0.5f;
        /// <summary>        /// ��ǰȼ�չ��ķ�Χ        /// </summary>
        public float burnRange = 0;

        protected virtual void Start()
        {
            fireValue = 0;
            burnRange = 0;
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

        /// <summary>
        /// ��ʼȼ�շ�������������忪ʼȼ�գ�ȼ��ֵΪ���ֵ
        /// </summary>
        public virtual void BeginFire()
        {
            fireValue = Random.Range(0.0f, 1.0f);
        }
    }
}