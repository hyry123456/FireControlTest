using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect.Fire
{
    /// <summary>
    /// ����ȼ�շ���ִ���õĸ������ݽṹ
    /// </summary>
    partial class FireSpeardTree
    {
        //��Ӱ�������
        public List<FireRoutineBase> speardList;
        //Ӱ����б�
        public List<float> effectSizes;
        //�Լ�
        public FireRoutineBase fire;
    }

    /// <summary>
    /// ��������࣬����Ϊ�Ӽ���ÿһ���������ȼ��
    /// </summary>
    public class FireMangent : MonoBehaviour
    {
        List<FireRoutineBase> mangentList;
        public float spreadRange = 10;
        private List<FireSpeardTree> trees;

        private void Awake()
        {
            int size = transform.childCount;
            mangentList = new List<FireRoutineBase>();
            trees = new List<FireSpeardTree>();
            for (int i = 0; i < size; i++)
            {
                FireRoutineBase routine = transform.GetChild(i).GetComponent<FireRoutineBase>();
                if(routine != null)
                    mangentList.Add(routine);
            }
            //���뷽���������б���
            Common.LoadScene.LoadSceneQueue.Instance.PutFuctionOnLoad(ReadLoadTree);
        }

        //�������õ���ʱ����
        int nowIndex;
        /// <summary>
        /// ����ִ��������ʱִ�е��㷨���ڳ�����ʼʱ����
        /// </summary>
        /// <returns>�Ƿ����</returns>
        public bool ReadLoadTree()
        {
            
            for(; nowIndex<mangentList.Count; )
            {
                FireSpeardTree fireSpeardTree = new FireSpeardTree();
                for(int i=0; i<mangentList.Count; i++)
                {
                    float spread = 1.0f - Mathf.Clamp01(
                        (mangentList[i].transform.position - mangentList[nowIndex].transform.position).sqrMagnitude
                        / spreadRange
                        );
                    if(spread != 0)
                    {
                        if (fireSpeardTree.speardList == null)
                        {
                            //�����Ƕ�Ӧ��
                            fireSpeardTree.speardList = new List<FireRoutineBase>();
                            fireSpeardTree.effectSizes = new List<float>();
                        }
                        fireSpeardTree.speardList.Add(mangentList[i]);
                        fireSpeardTree.effectSizes.Add(spread);
                    }
                }
                //ȷ���Լ�
                fireSpeardTree.fire = mangentList[nowIndex];
                //��ʱ��ֱ��ȼ��
                fireSpeardTree.fire.BeginFire();
                //����ȫ�������ٲ���
                if(fireSpeardTree.speardList != null)
                    trees.Add(fireSpeardTree);

                nowIndex++;
                return false;
            }
            mangentList.Clear();
            mangentList = null;
            return true;
        }

        //ˢ���������õ���ʱ����
        int treeIndex = 0;
        private void Update()
        {
            if(trees != null)
            {
                for(;treeIndex<trees.Count;)
                {
                    for(int i=0; i<trees[treeIndex].speardList.Count; i++)
                    {
                        trees[treeIndex].speardList[i].AddBurn(
                            trees[treeIndex].fire.fireValue, trees[treeIndex].effectSizes[i]
                            );
                    }
                    //ִ��ˢ�·���
                    trees[treeIndex].fire.RotinuFuction(trees.Count);
                    //���¶���Ϊ�˱�֤ʱʱִ��
                    treeIndex++;
                    treeIndex %= trees.Count;
                    return;
                }
            }

        }
    }
}