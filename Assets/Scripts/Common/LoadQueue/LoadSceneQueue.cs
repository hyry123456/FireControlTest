using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.LoadScene
{
    /// <summary>
    /// 逐帧加载场景的方法，用来场景加载时逐帧调用
    /// </summary>
    /// <returns>是否完成</returns>
    public delegate bool OnUpdateLoadScene();

    /// <summary>
    /// 加载场景类，用来执行复杂的加载任务时执行的方法，
    /// 可以确定当前已经加载的情况，同时分配到多帧执行，不会死机
    /// </summary>
    public class LoadSceneQueue : MonoBehaviour
    {
        private static LoadSceneQueue instance;
        public static LoadSceneQueue Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject go = new GameObject("LoadScene");
                    instance = go.AddComponent<LoadSceneQueue>();
                    go.hideFlags = HideFlags.HideAndDontSave;
                }
                return instance;
            }
        }

        private Stack<OnUpdateLoadScene> loadStatck;

        private void Awake()
        {
            loadStatck = new Stack<OnUpdateLoadScene>();
        }

        /// <summary>
        /// 准备加载场景，在1秒内的等待时间开始场景的加载行为，先放着先，之后完善
        /// </summary>
        public IEnumerator ReadyLoadScene()
        {
            //开始准备执行加载
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(LoadScene());
        }

        /// <summary>
        /// 将需要执行的方法放入执行栈中准备执行
        /// </summary>
        /// <param name="putFuction"></param>
        public void PutFuctionOnLoad(OnUpdateLoadScene putFuction)
        {
            loadStatck.Push(putFuction);
        }

        private IEnumerator LoadScene()
        {
            while (loadStatck.Count > 0)
            {
                OnUpdateLoadScene putFuction = loadStatck.Peek();
                //完成后就抛弃该方法
                if (putFuction())
                    loadStatck.Pop();
                yield return null;
            }
            yield break;
        }

        /// <summary>
        /// 确定是否场景正在加载中
        /// </summary>
        /// <returns>是否加载中</returns>
        public bool IsLoading()
        {
            return (loadStatck.Count != 0);
        }

    }
}