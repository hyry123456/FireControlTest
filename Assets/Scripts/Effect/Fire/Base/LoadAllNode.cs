using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// 火焰的加载类，用来将所有需要运行的子类准备好，扔给管理类进行运行
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
        /// 用来给每一个节点设置其需要的数据，目前只有粒子，因此传入粒子数据
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