using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>    /// �������ӿ��Ƶĵ����ڵ㣬�������ö�Ӧ���ӵĲ���    /// </summary>
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

        /// <summary>       /// ����ǿ�ȣ�Ҳ����Ŀǰȼ�վ������        /// </summary>
        public float fireIntensity;
        private float arrivef;
        /// <summary>        /// �����������ٶȣ�Ҳ����1����ٸ�        /// </summary>
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
            arrivef += Time.deltaTime * fireIntensity;      //ǿ��Ӱ����������ٶ�
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