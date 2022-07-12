using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.LoadScene
{
    /// <summary>
    /// ��֡���س����ķ�����������������ʱ��֡����
    /// </summary>
    /// <returns>�Ƿ����</returns>
    public delegate bool OnUpdateLoadScene();

    /// <summary>
    /// ���س����࣬����ִ�и��ӵļ�������ʱִ�еķ�����
    /// ����ȷ����ǰ�Ѿ����ص������ͬʱ���䵽��ִ֡�У���������
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
        /// ׼�����س�������1���ڵĵȴ�ʱ�俪ʼ�����ļ�����Ϊ���ȷ����ȣ�֮������
        /// </summary>
        public IEnumerator ReadyLoadScene()
        {
            //��ʼ׼��ִ�м���
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(LoadScene());
        }

        /// <summary>
        /// ����Ҫִ�еķ�������ִ��ջ��׼��ִ��
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
                //��ɺ�������÷���
                if (putFuction())
                    loadStatck.Pop();
                yield return null;
            }
            yield break;
        }

        /// <summary>
        /// ȷ���Ƿ񳡾����ڼ�����
        /// </summary>
        /// <returns>�Ƿ������</returns>
        public bool IsLoading()
        {
            return (loadStatck.Count != 0);
        }

    }
}