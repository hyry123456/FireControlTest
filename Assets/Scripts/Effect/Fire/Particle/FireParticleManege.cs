using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomRP.GPUPipeline;

namespace FireControl.Effect
{
    /// <summary>    /// 粒子火焰的控制类，本质上是一个控制类，由火焰管理类直接控制    /// </summary>
    public class FireParticleManege : FireRoutineBase
    {
        public ParticlePerObject[] particlePerObjects;
        protected FirePartNode[] firePartNodes;
        private ParticleGroup particleGroup;

        protected override void Start()
        {
            base.Start();
            particleGroup = GetComponent<ParticleGroup>();
            firePartNodes = GetComponentsInChildren<FirePartNode>();
            particlePerObjects = new ParticlePerObject[firePartNodes.Length];

            for (int i = 0; i < firePartNodes.Length; i++)
            {
                firePartNodes[i].ParticleManege = this;
                particlePerObjects[i] = new ParticlePerObject
                {
                    beginPos = firePartNodes[i].transform.position,
                    arriveIndex = 0,
                };
            }
            particleGroup.ReadyBuffer(particlePerObjects);
        }

        public override void AddBurn(float fireValue, float distance)
        {
        }

        public override void RotinuFuction(int nodeCount)
        {
            for (int i = 0; i < firePartNodes.Length; i++)
            {
                firePartNodes[i].RotinuFuction(i);
            }
            particleGroup.UpdatePerObjectsBuffer(particlePerObjects);
        }
    }
}