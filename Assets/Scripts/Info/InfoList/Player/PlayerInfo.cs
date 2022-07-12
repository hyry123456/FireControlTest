using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.Info
{

    /// <summary>
    /// 主角的信息类
    /// </summary>
    public class PlayerInfo : CharacterInfo
    {
        protected CharacterController controller;
        /// <summary>        /// 跳跃强度,也就是向上速度        /// </summary>
        public float jumpForce = 15;

        protected override void OnEnable()
        {
            base.OnEnable();
            //改变数据类型
            characterAction = new PlayerAction();
            controller = GetComponentInChildren<CharacterController>();
        }

        protected override void OnCharacterDie()
        {
            //base.OnCharacterDie();
        }

        protected override void OnHurt()
        {
            base.OnHurt();
            Debug.Log("主角受伤");
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>        /// 获得模型的真实坐标        /// </summary>
        public override Transform truthTransform
        {
            get
            {
                return controller.transform;
            }
        }
    }
}