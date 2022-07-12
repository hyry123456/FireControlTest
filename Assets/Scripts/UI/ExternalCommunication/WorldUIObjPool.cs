using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    /// <summary>
    /// ����UI����أ���������һЩ����UI��Ҫע��
    /// </summary>
    public class WorldUIObjPool : MonoBehaviour
    {
        Queue<GameObject> simpleObj = new Queue<GameObject>();
        public GameObject simpleWorldUi;
        void Start()
        {
            simpleWorldUi = Resources.Load<GameObject>("UI/Image_WorldUISimple");
            if (simpleWorldUi == null) Debug.Log("û�м��ص�����UI���");
        }

        /// <summary>
        /// ��ȡ�򵥵�����UI
        /// </summary>
        public GameObject GetSimpleGameObject()
        {
            GameObject re;
            if (simpleObj.Count == 0)
            {
                re = GameObject.Instantiate(simpleWorldUi);
                re.transform.parent = transform;
                return re;
            }
            else
            {
                re = simpleObj.Dequeue();
            }
            re.SetActive(true);
            Debug.Log(re.name);
            return re;
        }

        public void AddSimpleGameObject(GameObject gameObject)
        {
            simpleObj.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
}