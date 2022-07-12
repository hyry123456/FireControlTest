using System.Collections.Generic;
using UnityEngine;

namespace Common.ObjectPool
{
    /// <summary>
    /// ��Ϸ������࣬�ø���ȥ��������
    /// </summary>
    public class GameObjectPool : MonoBehaviour
    {
        static GameObjectPool objectPool;
        private Dictionary<string, Queue<GameObject>> objectQueueDictionary;
        /// <summary>        /// �����洢�����������λ��        /// </summary>
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
        /// ����һ�������������������ʹ��ڣ��Ͳ�������
        /// </summary>
        /// <param name="origen">Դ����</param>
        /// <param name="name">��������</param>
        /// <param name="suvivalTime">���ʱ��</param>
        /// <returns>�����Ķ���</returns>
        public GameObject GetObject(GameObject origen, string name, float suvivalTime)
        {
            if (origen == null) return null;
            Queue<GameObject> games;
            GameObject game = null;
            //������û���������
            if (objectQueueDictionary.ContainsKey(name))
            {
                games = objectQueueDictionary[name];
                //������ʣ���
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
                //û��ʱ����һ��
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
        /// ������Żض���أ����Զ�������״̬Ϊ�ر�״̬
        /// </summary>
        /// <param name="insertObj">Ҫ����Ķ���</param>
        /// <param name="name">����</param>
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
        /// �ı䳡��ʱ���õĺ����������������ഴ���Ķ��󣬶Ը�����г�ʼ��
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