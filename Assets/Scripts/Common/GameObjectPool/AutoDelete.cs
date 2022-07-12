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
        /// 当该物体存活时间小于0，表示这个物体的存活不受时间控制
        /// 由其他对象调用删除时才会删除
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