using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    /// <summary>
    /// 世界UI对象池，用来创建一些世界UI，要注意
    /// </summary>
    public class WorldUIObjPool : MonoBehaviour
    {
        Queue<GameObject> simpleObj = new Queue<GameObject>();
        public GameObject simpleWorldUi;
        void Start()
        {
            simpleWorldUi = Resources.Load<GameObject>("UI/Image_WorldUISimple");
            if (simpleWorldUi == null) Debug.Log("没有加载到世界UI组件");
        }

        /// <summary>
        /// 获取简单的世界UI
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