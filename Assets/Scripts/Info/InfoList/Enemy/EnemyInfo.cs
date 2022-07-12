
using UnityEngine;

namespace FireControl.Info
{
    /// <summary>
    /// 怪物信息类，用来对怪物的信息进行拓展
    /// </summary>
    public class EnemyInfo : CharacterInfo
    {
        /// <summary>        /// 攻击距离的平方        /// </summary>
        public float attackSqrDis = 4f;

        protected CharacterController controller;

        protected override void OnEnable()
        {
            base.OnEnable();
            controller = GetComponentInChildren<CharacterController>();
        }

        /// <summary>
        /// 重写血量减少时调用的方法，用来锁定攻击敌人的目标
        /// </summary>
        protected override int OnHPChange(int changeSize, CharacterInfo info)
        {
            changeSize = base.OnHPChange(changeSize, info);
            AI.FightAIStateMachine ai = GetComponent<AI.FightAIStateMachine>();
            if (ai == null)
                return changeSize;
            ai.AddAttackInfo(info, changeSize);
            return changeSize;
        }

        public override Transform truthTransform
        {
            get
            {
                return controller.transform;
            }
        }

        public override Quaternion truthRotaion
        {
            get
            {
                return controller.transform.rotation;
            }
            set
            {
                controller.transform.rotation = value;
            }
        }
    }
}