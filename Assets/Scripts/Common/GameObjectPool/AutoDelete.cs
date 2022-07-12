using System.Collections;
using UnityEngine;


namespace Common.ObjectPool
{
    public class AutoDelete : MonoBehaviour
    {
        public float survivalTime = 5;
        public string saveName;
        public bool isDestory = false;
        /// <summary>
        /// ����������ʱ��С��0����ʾ�������Ĵ���ʱ�����
        /// �������������ɾ��ʱ�Ż�ɾ��
        /// </summary>
        public void OnEnable()
        {
            if (survivalTime < 0) return;
            StartCoroutine(AutoDeleteObject());
        }

        private IEnumerator AutoDeleteObject()
        {
            yield return new WaitForSeconds(survivalTime);
            if(!isDestory)
                GameObjectPool.Instance.InsertObject(gameObject, saveName);
            else
                Destroy(gameObject);
        }

    }
}