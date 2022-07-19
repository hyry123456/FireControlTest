using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomRP.GPUPipeline;

namespace FireControl.Effect
{
    /// <summary>    /// ���ӻ���Ŀ����࣬��������һ�������࣬�ɻ��������ֱ�ӿ���    /// </summary>
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