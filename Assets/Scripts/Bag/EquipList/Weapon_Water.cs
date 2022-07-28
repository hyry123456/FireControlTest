using UnityEngine;
using Common.ParticleSystem;

namespace FireControl.Equip
{
    public class Weapon_Water : Weapon
    {
        public float suviverTime;
        public float waterGunforce = 5f;

        ParticleWater particleWater;

        public Weapon_Water()
        {
            Initialize();
        }

        public override EquipReturn WeaponBehavior(Bag bag)
        {
            //Debug.Log("运行了水枪");

            equipNeogor--;
            if (particleWater == null)
            {
                particleWater = Common.CommonFunction.GetComponentInChild<ParticleWater>(bag.transform);
            }
            EquipReturn equipReturn = new EquipReturn();
            //我们目前武器没有特殊效果，所以不管，之后一些装备需要添加
            //buff就在这里加
            equipReturn.addState = null;
            //该武器是非伤害武器，同时其实一般的伤害也不是在这里加的
            //如果是治疗道具才需要在这里返回非零值
            equipReturn.value = attackValue;
            if (particleWater == null)
            {
                Debug.Log("没找到");
                return equipReturn;
            }

            GameObject gameObject = particleWater.RunWater();
            if(gameObject != null)
            {
                Effect.ParticleNode fireNode = gameObject.GetComponent<Effect.ParticleNode>();
                if (fireNode != null)
                    fireNode.AddFireIntensity(-1);
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