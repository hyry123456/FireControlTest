using System.Collections;
using UnityEngine;

namespace FireControl.Interaction
{
    public abstract class InteractionInfo : MonoBehaviour
    {
        [HideInInspector]
        public InteractionType interactionType;
        /// <summary>        /// ��Ҫע�⣬����IDһ�㲻�������ر�ָ����������һЩ��������½�����ʱ��ֵ�õ�
        /// Ҳ����˵��ʼ������Ҫ��ֵ������/// </summary>
        public int interactionID = 0;
        /// <summary>        /// ��ʼ�������ƣ�ID��ֵ̫�鷳�ˣ���������        /// </summary>
        public string interactionName;

        /// <summary>
        /// ��ʼ��InteractionType����interactionName
        /// </summary>
        protected abstract void OnEnable();

        /// <summary>
        /// �ý�����Ϊ��Ҫ�ɵ�����
        /// </summary>
        /// <param name="interControl">�����������</param>
        /// <returns>Э�̷Żأ�������ʱ</returns>
        public abstract IEnumerator InteractionBehavior(InteractionControl interControl);

    }
}