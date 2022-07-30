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
            //我们目前武器没有特殊效果，所以不管，之后一些装备需要添加
            //buff就在这里加
            equipReturn.addState = null;
            //该武器是非伤害武器，同时其实一般的伤害也不是在这里加的
            //如果是治疗道具才需要在这里返回非零值
            equipReturn.value = attackValue;
            if (noiseWater == null)
            {
                Debug.Log("没找到");
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
            this.equipName = "水枪";
            this.equipIntroduce = "一把十分牛逼的水枪";
            this.maxNeogor = 100;
            this.equipNeogor = 0;
            this.className = "Weapon_Water";
        }
    }
}