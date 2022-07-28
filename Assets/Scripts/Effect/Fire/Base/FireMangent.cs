using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// ���������㷨�漰�Ĵ洢�ṹ�壬�����洢���еĻ�������
    /// </summary>
    public struct FireMap
    {
        public FireNodeBase[] allNodes;        //���еĽڵ�
        public List<KeyValuePair<int, float>>[] affectMap;      //�໥Ӱ��Ĺ�ϵ��

        /// <summary>
        /// ����Ӱ���ϵ������ȷ������֮����໥Ӱ��Ч���������ӵ����̼߳���
        /// </summary>
        public void ReadyNodes()
        {
            if (allNodes == null || allNodes.Length == 0) return;
            affectMap = new List<KeyValuePair<int, float>>[allNodes.Length];
            //��鶼�Ǻ���ǰ���������ظ�����
            for(int i=0; i<allNodes.Length; i++)
            {
                Vector3 pos = allNodes[i].transform.position;
                affectMap[i] = new List<KeyValuePair<int, float>>();
                for(int j=0; j<i; j++)
                {
                    Vector3 thisPos = allNodes[j].transform.position;
                    float maxRange = Mathf.Max(allNodes[j].burnRange, allNodes[i].burnRange);
                    maxRange *= maxRange;   //ƽ�������ٿ�����������
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
    /// ��������࣬����׼��ȼ�յ�Ӱ���ϵ����ֱ�ӹ���ÿһ�������ȼ�գ�
    /// �����ȼ�շ���ʵ�������ɸ��ڵ��ϵķ���������
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
        /// �������Э�̶��еķ������÷�����һֱ��Э���е��ã�����ֹͣ
        /// </summary>
        public bool RunNodes()
        {
            for(int i=0; i<fireMap.allNodes.Length; i++)
            {
                //����ÿһ��ȼ�յ�ʵʱ����
                fireMap.allNodes[i].BurnUpdate();
            }

            fireMap.allNodes[nowIndex].RotinuFuction(fireMap, nowIndex);
            nowIndex++;
            nowIndex %= fireMap.allNodes.Length;
            return false;
        }

    }
}