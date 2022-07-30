using UnityEngine;
using CustomRP.GPUPipeline;

namespace FireControl.Equip
{
    public class Weapon_Water : Weapon
    {
        public float suviverTime;
        public float waterGunforce = 5f;

        //ParticleWater particleWater;
        NoiseWater noiseWater;

        public Weapon_Water()
        {
            Initialize();
        }

        public override EquipReturn WeaponBehavior(Bag bag)
        {

            equipNeogor--;
            if (noiseWater == null)
            {
                noiseWater = bag.gameObject.GetComponentInChildren<NoiseWater>();
            }
            EquipReturn equipReturn = new EquipReturn();
            //����Ŀǰ����û������Ч�������Բ��ܣ�֮��һЩװ����Ҫ���
            //buff���������
            equipReturn.addState = null;
            //�������Ƿ��˺�������ͬʱ��ʵһ����˺�Ҳ����������ӵ�
            //��������Ƶ��߲���Ҫ�����ﷵ�ط���ֵ
            equipReturn.value = attackValue;
            if (noiseWater == null)
            {
                Debug.Log("û�ҵ�");
                return equipReturn;
            }

            GameObject gameObject = noiseWater.RunWater();
            if(gameObject != null)
            {
                Effect.ParticleNode fireNode = gameObject.GetComponent<Effect.ParticleNode>();
                if (fireNode != null)
                {
                    fireNode.AddFireIntensity(-5);
                }
            }
            return equipReturn;
        }

        protected override void Initialize()
        {
            this.equipID = 0;
            this.attackValue = 0;
            this.suviverTime = 4f;
            this.equipType = EquipType.Weapon;
            this.equipName = "ˮǹ";
            this.equipIntroduce = "һ��ʮ��ţ�Ƶ�ˮǹ";
            this.maxNeogor = 100;
            this.equipNeogor = 0;
            this.className = "Weapon_Water";
        }
    }
}