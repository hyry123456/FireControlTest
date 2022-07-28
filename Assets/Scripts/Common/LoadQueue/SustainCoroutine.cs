using System.Collections;
using UnityEngine;

namespace Common
{
    /// <summary>    /// 用来给协程运行用的方法，返回true时死亡    /// </summary>
    public delegate bool CoroutinesAction();

    /// <summary>
    /// 配合携程加载用的数组，用池来优化协程的插入以及删除
    /// </summary>
    class SustainList
    {
        public CoroutinesAction[] coroutines = new CoroutinesAction[1];
        public int size = 0;

        public void Add(CoroutinesAction coroutine)
        {
            CoroutinesAction[] newCorutines;
            if (size == coroutines.Length)
            {
                newCorutines = new CoroutinesAction[size + 5];
                for (int i = 0; i < size; i++)
                    newCorutines[i] = coroutines[i];
                size = newCorutines.Length;
                coroutines = newCorutines;
            }
            coroutines[size] = coroutine;
            size++;
        }

        /// <summary>   /// 移除传入编号的协程    /// </summary>
        public void Remove(int removeIndex)
        {
            if (removeIndex >= size) return;
            coroutines[removeIndex] = coroutines[size - 1];
            size--;
        }
    }


    public class SustainCoroutine : MonoBehaviour
    {
        private static SustainCoroutine instance;
        public static SustainCoroutine Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("SustainCoroutine");
                    gameObject.AddComponent<SustainCoroutine>();
                    DontDestroyOnLoad(gameObject);
                }
                return instance;
            }
        }
        private bool isRunning = false;
        private SustainList sustainList = new SustainList();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            isRunning = true;
            sustainList = new SustainList();
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (isRunning)
            {
                if (sustainList.size == 0)
                    yield return new WaitForSeconds(1);

                for (int i = sustainList.size - 1; i >= 0; i--)
                {
                    if (sustainList.coroutines[i]())
                        sustainList.Remove(i);
                }
                yield return null;
            }
        }

        public void AddCoroutine(CoroutinesAction action)
        {
            sustainList.Add(action);
        }

        private void OnDisable()
        {
            isRunning = false;
        }
    }

}
