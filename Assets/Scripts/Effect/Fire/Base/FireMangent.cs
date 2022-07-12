using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect.Fire
{
    /// <summary>
    /// 用于燃烧方法执行用的根据数据结构
    /// </summary>
    partial class FireSpeardTree
    {
        //受影响的数组
        public List<FireRoutineBase> speardList;
        //影响的列表
        public List<float> effectSizes;
        //自己
        public FireRoutineBase fire;
    }

    /// <summary>
    /// 火焰管理类，用于为子集的每一个物体进行燃烧
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
            //插入方法到加载列表中
            Common.LoadScene.LoadSceneQueue.Instance.PutFuctionOnLoad(ReadLoadTree);
        }

        //树加载用的临时数据
        int nowIndex;
        /// <summary>
        /// 用来执行树加载时执行的算法，在场景开始时加载
        /// </summary>
        /// <returns>是否结束</returns>
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
                            //两个是对应的
                            fireSpeardTree.speardList = new List<FireRoutineBase>();
                            fireSpeardTree.effectSizes = new List<float>();
                        }
                        fireSpeardTree.speardList.Add(mangentList[i]);
                        fireSpeardTree.effectSizes.Add(spread);
                    }
                }
                //确定自己
                fireSpeardTree.fire = mangentList[nowIndex];
                //暂时先直接燃烧
                fireSpeardTree.fire.BeginFire();
                //加载全部后在再插入
                if(fireSpeardTree.speardList != null)
                    trees.Add(fireSpeardTree);

                nowIndex++;
                return false;
            }
            mangentList.Clear();
            mangentList = null;
            return true;
        }

        //刷新树数据用的临时数据
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
                    //执行刷新方法
                    trees[treeIndex].fire.RotinuFuction(trees.Count);
                    //以下都是为了保证时时执行
                    treeIndex++;
                    treeIndex %= trees.Count;
                    return;
                }
            }

        }
    }
}