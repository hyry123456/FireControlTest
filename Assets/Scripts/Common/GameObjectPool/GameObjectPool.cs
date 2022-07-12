using System.Collections.Generic;
using UnityEngine;

namespace Common.ObjectPool
{
    /// <summary>
    /// 游戏对象池类，用该类去创建对象
    /// </summary>
    public class GameObjectPool : MonoBehaviour
    {
        static GameObjectPool objectPool;
        private Dictionary<string, Queue<GameObject>> objectQueueDictionary;
        /// <summary>        /// 用来存储创建的物体的位置        /// </summary>
        private static GameObject createParent;
        public static GameObjectPool Instance
        {
            get
            {
                if (objectPool == null)
                {
                    GameObject go = new GameObject("ObjectPool");
                    go.hideFlags = HideFlags.DontSave;
                    objectPool = go.AddComponent<GameObjectPool>();
                }
                return objectPool;
            }
        }

        private void OnEnable()
        {
            objectQueueDictionary = new Dictionary<string, Queue<GameObject>>();
            createParent = this.gameObject;
        }

        /// <summary>
        /// 创建一个对象，如果这个对象本来就存在，就不会生成
        /// </summary>
        /// <param name="origen">源对象</param>
        /// <param name="name">对象名称</param>
        /// <param name="suvivalTime">存活时间</param>
        /// <returns>创建的对象</returns>
        public GameObject GetObject(GameObject origen, string name, float suvivalTime)
        {
            if (origen == null) return null;
            Queue<GameObject> games;
            GameObject game = null;
            //看看有没有这个队列
            if (objectQueueDictionary.ContainsKey(name))
            {
                games = objectQueueDictionary[name];
                //有且有剩余的
                if (games.Count > 0)
                {
                    game = games.Peek();
                    games.Dequeue();
                }
            }
            if(game == null)
            {
                if(createParent == null)
                {
                    createParent = GameObject.FindGameObjectWithTag("ObjectPool");
                }
                //没有时创造一个
                game = GameObject.Instantiate(origen, createParent.transform);
            }
            AutoDelete auto = game.GetComponent<AutoDelete>();
            if(auto == null)
                auto = game.AddComponent<AutoDelete>();
            auto.saveName = name;
            auto.survivalTime = suvivalTime;
            game.SetActive(true);
            return game;
        }

        /// <summary>
        /// 将对象放回对象池，会自动设置其状态为关闭状态
        /// </summary>
        /// <param name="insertObj">要放入的对象</param>
        /// <param name="name">名称</param>
        public void InsertObject(GameObject insertObj, string name)
        {
            Queue<GameObject> games;
            insertObj.SetActive(false);
            if (objectQueueDictionary.TryGetValue(name, out games))
            {
                games.Enqueue(insertObj);
                return;
            }
            games = new Queue<GameObject>();
            games.Enqueue(insertObj);
            objectQueueDictionary.Add(name, games);
        }

        /// <summary>
        /// 改变场景时调用的函数，用来清空这个类创建的对象，对该类进行初始化
        /// </summary>
        public void ChangeScene()
        {
            Destroy(createParent);
            createParent = null;
            Destroy(objectPool);
            objectPool = null;
        }
    }
}