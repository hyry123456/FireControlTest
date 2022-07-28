using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// 火焰生成算法涉及的存储结构体，用来存储所有的火焰数据
    /// </summary>
    public struct FireMap
    {
        public FireNodeBase[] allNodes;        //所有的节点
        public List<KeyValuePair<int, float>>[] affectMap;      //相互影响的关系表

        /// <summary>
        /// 生成影响关系表，用来确定火焰之间的相互影响效果，建议扔到多线程加载
        /// </summary>
        public void ReadyNodes()
        {
            if (allNodes == null || allNodes.Length == 0) return;
            affectMap = new List<KeyValuePair<int, float>>[allNodes.Length];
            //检查都是后往前看，避免重复叠加
            for(int i=0; i<allNodes.Length; i++)
            {
                Vector3 pos = allNodes[i].transform.position;
                affectMap[i] = new List<KeyValuePair<int, float>>();
                for(int j=0; j<i; j++)
                {
                    Vector3 thisPos = allNodes[j].transform.position;
                    float maxRange = Mathf.Max(allNodes[j].burnRange, allNodes[i].burnRange);
                    maxRange *= maxRange;   //平方来减少开发的运算量
                    float dis = (pos - thisPos).sqrMagnitude;
                    if (dis < maxRange)
                    {
                        affectMap[i].Add(new KeyValuePair<int, float>(j, dis / maxRange));
                    }
                }
            }
        }
    }
    /// <summary>
    /// 火焰管理类，用于准备燃烧的影响关系，不直接管理每一个物体的燃烧，
    /// 具体的燃烧方法实际上是由根节点上的方法决定的
    /// </summary>
    public class FireMangent 
    {
        private static FireMangent instance;
        public static FireMangent Instance
        {
            get
            {
                if (instance == null)
                    instance = new FireMangent();
                return instance;
            }
        }

        private FireMap fireMap;
        private FireMangent()
        {
            fireMap = new FireMap();
        }

        public void AddFireNode(FireNodeBase[] fireNodeBases)
        {
            fireMap.allNodes = fireNodeBases;
            fireMap.ReadyNodes();
            Common.SustainCoroutine.Instance.AddCoroutine(RunNodes);
        }

        int nowIndex = 0;
        /// <summary>
        /// 负责插入协程队列的方法，该方法会一直在协程中调用，不会停止
        /// </summary>
        public bool RunNodes()
        {
            for(int i=0; i<fireMap.allNodes.Length; i++)
            {
                //调用每一个燃烧的实时方法
                fireMap.allNodes[i].BurnUpdate();
            }

            fireMap.allNodes[nowIndex].RotinuFuction(fireMap, nowIndex);
            nowIndex++;
            nowIndex %= fireMap.allNodes.Length;
            return false;
        }

    }
}