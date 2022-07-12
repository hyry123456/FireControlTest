
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 人物常用方法，用来执行查找人物以及获取人物总父类
    /// </summary>
    public class CharacterFouction : MonoBehaviour
    {

        private static GameObject game;
        public static GameObject GetCharacter
        {
            get
            {
                if (game == null)
                {
                    game = GameObject.Find("Character");
                    if(game == null)
                    {
                        game = new GameObject("Character");
                        instance = game.AddComponent<CharacterFouction>();
                        game.hideFlags = HideFlags.None;
                    }
                }
                return game;
            }
        }

        private static CharacterFouction instance;
        public static CharacterFouction Instance
        {
            get
            {
                if (instance == null)
                {
                    game = GameObject.Find("Character");
                    if(game == null)
                    {
                        game = new GameObject("Character");
                        instance = game.AddComponent<CharacterFouction>();
                        game.hideFlags = HideFlags.None;
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 更新怪物列表，用来当怪物列表存在刷新时调用
        /// </summary>
        public static void UpdateEnemyList()
        {
            instance.StartCoroutine(instance.DelayGetEnemyList());
        }

        public static void RemoveEnemy(FireControl.AI.EnemyCreateMachine machine)
        {
            instance.enemies.Remove(machine);
        }


        /// <summary>
        /// 获取所有的敌人信息列表
        /// </summary>
        /// <returns>敌人数组</returns>
        public static List<FireControl.Info.EnemyInfo> GetAllEnemy()
        {
            List<FireControl.Info.EnemyInfo> re = new List<FireControl.Info.EnemyInfo>();
            for(int i=0; i<Instance.enemies.Count; i++)
                re.AddRange(Instance.enemies[i].GetComponentsInChildren<FireControl.Info.EnemyInfo>());
            if (Instance.dnaTransform == null) return re;
            re.AddRange(Instance.dnaTransform.GetComponentsInChildren<FireControl.Info.EnemyInfo>());
            return re;
        }

        private List<FireControl.AI.EnemyCreateMachine> enemies;
        private Transform dnaTransform;

        private void Awake()
        {
            if (game == null) game = gameObject;
            if(instance == null) instance = this;
            enemies = new List<FireControl.AI.EnemyCreateMachine>();
            UpdateEnemyList();
            CommonFunction.DelayFindInTransform("DnaEnemy", transform, (input) =>
            {
                dnaTransform = input;
            });
        }

        /// <summary>
        /// 用来刷新子数据
        /// </summary>
        private void FixedUpdate()
        {
            if (enemies == null) return;
            for(int i=0; i<enemies.Count; i++)
                enemies[i].OnFixedUpdate();
        }

        private IEnumerator OnUpdateFindObj<T>(Transform transform,
            Handler.HandlerList.IGetBoolByOneParam<Transform> judgeFun, Handler.HandlerList.ISetOneParam<T> setOneParam) where T : Component
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(transform);
            while (queue.Count > 0)
            {
                Transform child = queue.Dequeue();
                if (judgeFun(child))
                {
                    T t = child.GetComponent<T>();
                    setOneParam(t);
                    //由于查找的是人物，因此找到时就不用向下找了，下面一定没有
                    yield return null;
                    continue;
                }
                for (int i = 0; i < child.childCount; i++)
                {
                    queue.Enqueue(child.GetChild(i));
                }
                yield return null;
            }
            queue.Clear();
            queue = null;
        }

        private IEnumerator DelayGetEnemyList()
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(transform);
            while (queue.Count > 0)
            {
                Transform child = queue.Dequeue();
                if(child.GetComponent<FireControl.AI.EnemyCreateMachine>() != null && child.gameObject.activeSelf == true)
                {
                    enemies.Add(child.GetComponent<FireControl.AI.EnemyCreateMachine>());
                    yield return null;
                    //跳过子级，因为不会在有需要获取的了
                    continue;
                }
                for (int i = 0; i < child.childCount; i++)
                {
                    queue.Enqueue(child.GetChild(i));
                }
                yield return null;
            }
            queue.Clear();
            queue = null;
        }


        /// <summary>
        /// 查找符合要求的数据，然后设置这个数据，协程查找，优化速度
        /// </summary>
        /// <param name="transform">查找的总父物体</param>
        /// <param name="judgeFun">判断的方法</param>
        /// <param name="setOneParam">设置查找到的参数的方法</param>
        public void FindObjectsAndSetIt<T>(Transform transform,
            Handler.HandlerList.IGetBoolByOneParam<Transform> judgeFun, Handler.HandlerList.ISetOneParam<T> setOneParam) where T : Component
        {
            if (judgeFun == null || setOneParam == null)
            {
                Debug.Log("判断函数为null");
                return;
            }
            if (transform == null) { Debug.LogError("根据值为空"); return; }
            StartCoroutine(OnUpdateFindObj(transform, judgeFun, setOneParam));
        }


    }
}