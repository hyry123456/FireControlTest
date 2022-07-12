
using UnityEngine;


namespace FireControl.Skill
{
    /// <summary>/// 主角的翻滚技能，也就是闪避/// </summary>
    public class PlayerRoll : SkillBase
    {
        private Vector3 dir;
        private int stateIndex;
        private GameObject view;
        public PlayerRoll()
        {
            skillType = SkillType.Dodge;
            expendSP = 0;
            coolTime = 1.5f;
            relaseTime = 1f;
            skillName = "翻滚"; 
            view = GameObject.Find("View");
        }

        public override void OnSkillEnd(SkillManage mana)
        {
            Info.PlayerAction playerAction = mana.CharacterInfo.characterAction as Info.PlayerAction;
            //playerAction.OtherValue = 0;
            stateIndex = int.MaxValue;
            //playerAction.Dodge = false;
        }

        public override void OnSkillRelease(SkillManage mana)
        {
            Debug.Log("技能释放");
            stateIndex = 7;
            //添加无敌状态，用-1表示时间不定
            mana.CharacterInfo.AddState(-1, new Info.DodgeState());

            float vertical = Common.ResetInput.MyInput.Instance.GetAsis("Vertical");
            float horizontal = Common.ResetInput.MyInput.Instance.GetAsis("Horizontal");
            dir = Vector3.zero;
            if (vertical != 0 || horizontal != 0)
            {
                dir += Vector3.Lerp(-view.transform.forward, view.transform.forward, vertical * 0.5f + 0.5f);
                dir += Vector3.Lerp(-view.transform.right, view.transform.right, horizontal * 0.5f + 0.5f);
            }
            else
                dir = view.transform.forward;
            dir.y += 1;
            dir.Normalize();
        }

        public override void OnCommonSkillReleasing(SkillManage mana, float nowTime)
        {
            //退出闪避状态
            if ((stateIndex & 1) != 0)
            {
                if (nowTime/relaseTime <= 0.5)
                {
                    mana.CharacterInfo.ReMoveState(new Info.DodgeState());
                    Common.CommonFunction.SetBirIndexToZero(ref stateIndex, 0);
                }
                //if(nowTime/relaseTime >= 0.9)
                //{
                //    mana.Motor.MoveAndRotate(dir, mana.CharacterInfo.rotateSpeed * 2, 0);
                //}
            }
            //mana.Motor.MoveAndRotate(dir, mana.CharacterInfo.rotateSpeed, mana.CharacterInfo.walkSpeed);
            //Info.PlayerAction playerAction = mana.CharacterInfo.characterAction as Info.PlayerAction;
            //playerAction.OtherValue = 1;
            //playerAction.Dodge = true;
        }

        public override bool OnUnknowEndSkillReleasing(SkillManage mana)
        {
            return true;
        }
    }
}