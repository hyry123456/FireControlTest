using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// ����ļ����࣬������������Ҫ���е�����׼���ã��Ӹ��������������
    /// </summary>
    public class LoadAllNode : MonoBehaviour
    {
        public CustomRP.GPUPipeline.ParticleNoise particle;

        void Start()
        {
            LoadAllNodesAndInsert();
        }

        private void LoadAllNodesAndInsert()
        {
            //Queue<Transform> transforms = new Queue<Transform>();
            //List<FireNodeBase> fireNodeBases = new List<FireNodeBase>();
            //transforms.Enqueue(transform);
            //while(transforms.Count != 0)
            //{
            //    Transform t = transforms.Dequeue();
            //    FireNodeBase fireNode = t.GetComponent<FireNodeBase>();
            //    if(fireNode != null)
            //    {
            //        fireNodeBases.Add(fireNode);
            //        continue;
            //    }
            //    for(int i=0; i<t.childCount; i++)
            //    {
            //        Transform child = t.GetChild(i);
            //        if (child.gameObject.activeSelf)
            //            transforms.Enqueue(child);
            //    }
            //}

            List<FireNodeBase> fireNodeBases = new List<FireNodeBase>();
            for(int i=0; i< particle.noiseDatas.Length; i++)
            {
                FireNodeBase fireNodeBase = particle.noiseDatas[i].position.GetComponent<FireNodeBase>();
                if(fireNodeBase != null)
                {
                    fireNodeBases.Add(fireNodeBase);
                    ReadyNodes(fireNodeBase, i);
                }
            }
            FireMangent.Instance.AddFireNode(fireNodeBases.ToArray());
        }

        /// <summary>
        /// ������ÿһ���ڵ���������Ҫ�����ݣ�Ŀǰֻ�����ӣ���˴�����������
        /// </summary>
        private void ReadyNodes(FireNodeBase nodeBase, int index)
        {
            ParticleNode particleNode = nodeBase as ParticleNode;
            if(particleNode != null)
            {
                particleNode.particles = particle;
                particleNode.particleIndex = index;
            }
        }
    }
}