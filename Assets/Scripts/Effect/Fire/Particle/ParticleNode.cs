using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect
{
    public class ParticleNode : FireNodeBase
    {
        float arriveF = 0;

        [System.NonSerialized]
        public int particleIndex = -1;
        [System.NonSerialized]
        public CustomRP.GPUPipeline.ParticleNoise particles;
        public override void RotinuFuction(FireMap fireMap, int index)
        {
            List<KeyValuePair<int, float>> affects = fireMap.affectMap[index];
            for(int i=0; i<affects.Count; i++)
            {
                float dis = FireIntensity - fireMap.allNodes[affects[i].Key].FireIntensity;
                //添加燃烧节点的燃烧值
                fireMap.allNodes[affects[i].Key].AddFireIntensity(
                    affects[i].Value * FireIntensity * Time.deltaTime * dis);
                //添加自己的燃烧值
                AddFireIntensity(affects[i].Value * fireMap.allNodes[affects[i].Key].FireIntensity * -dis);
            }
        }

        public override void BurnUpdate()
        {
            base.BurnUpdate();
            if (particles == null) return;

            arriveF += Time.deltaTime * fireIntensity;
            uint add = (uint)(arriveF / (1.0f / particles.ParticleOutCount));
            if (add > 0)
            {
                particles.init[particleIndex].arriveIndex += add;
                arriveF = 0;
                particles.init[particleIndex].arriveIndex %= 1000000007;
            }

        }
    }
}