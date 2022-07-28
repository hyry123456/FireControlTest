using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// ���з����Ļ��࣬���㷽������
    /// </summary>
    public abstract class FireNodeBase : MonoBehaviour
    {
        /// <summary>        
        /// �Ѿ�ȼ�յĻ���ֵ����Χ��0-100
        /// </summary>
        public float fireValue = 0;
        /// <summary>        /// ȼ�շ�Χ������ȷ������Ӱ�쵽������        /// </summary>
        public float burnRange = 1;
        [Range(0, 0.1f)]
        /// <summary>        /// ��ȼ��        /// </summary>
        public float inflammability = 0.05f;
        /// <summary>        /// ����ǿ�ȣ�Ҳ���ǻ���������ٶȣ���Χ��0-1     /// </summary>
        public float fireIntensity;

        public float FireIntensity
        {
            get { return fireIntensity; }
        }

        protected virtual void Start()
        {
            //֮����Գ���ʹ���ı�����
            fireValue = Random.Range(0f, 20f);
            fireIntensity = Random.Range(0.0f, 0.5f);
        }

        /// <summary>
        /// ����ȼ����Χ�����Լ�����ķ��������жԿ�Ӱ�������ȼ���Լ������ȼ�գ�
        /// �÷�����������֡��������������˽���ֻ����ȼ�ո�ֵ
        /// </summary>
        /// <param name="fireMap">����ȼ������Ĵ洢�ṹ��</param>
        /// <param name="index">������ı��</param>
        public abstract void RotinuFuction(FireMap fireMap, int index);

        /// <summary>
        /// �Լ�ȼ�յķ������÷�������֡���õģ���������Լ���ȼ������Լ�һЩ��֡����Ч,
        /// ����ʵ���˻������ȼ����
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