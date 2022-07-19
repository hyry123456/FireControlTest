using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>    /// 火焰粒子控制的当个节点，用来设置对应粒子的参数    /// </summary>
    public class FirePartNode : FireRoutineBase
    {
        private FireParticleManege particleManege;
        public FireParticleManege ParticleManege
        {
            set
            {
                this.particleManege = value;
            }
        }

        /// <summary>       /// 火焰强度，也就是目前燃烧剧烈情况        /// </summary>
        public float fireIntensity;
        private float arrivef;
        /// <summary>        /// 粒子上升的速度，也就是1秒多少个        /// </summary>
        public float partUpMaxSpeed = 10f;

        protected override void Start()
        {
            fireValue = Random.Range(0.0001f, 0.3f);
            fireIntensity = Random.Range(0.1f, 0.3f);
        }
        public override void AddBurn(float fireValue, float distance)
        {
            throw new System.NotImplementedException();
        }

        public override void RotinuFuction(int nodeCount)
        {
            arrivef += Time.deltaTime * fireIntensity;      //强度影响控制上升速度
            if (arrivef / (1.0f/ partUpMaxSpeed) > 1)
            {
                arrivef = 0;
                particleManege.particlePerObjects[nodeCount].arriveIndex++;
                //Debug.Log(nodeCount.ToString() + "  " + particleManege.particlePerObjects[nodeCount].arriveIndex);
            }
        }

        public void DecreaseFireIntensity()
        {
            fireIntensity -= Time.deltaTime * 0.1f;
            fireIntensity = Mathf.Clamp01(fireIntensity);
        }
    }
}