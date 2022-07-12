
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.Skill
{
    /// <summary>
    /// 主角攻击
    /// </summary>
    public class PlayetAttack : SkillBase
    {
        public PlayetAttack()
        {
            skillType = SkillType.NearDisAttack;
            expendSP = 0;
            coolTime = 2f;
            relaseTime = 1f;
            skillName = "主角攻击";
            skillMaxRange = 4f;
        }
        public override void OnSkillEnd(SkillManage mana)
        {
            mana.Anima.choseActionByParam = null;
            //mana.CharacterInfo.characterAction.Attack = 0;

        }

        public override void OnSkillRelease(SkillManage mana)
        {
            if (mana.Anima == null) Debug.LogWarning("没有动画");
            //mana.CharacterInfo.characterAction.Attack = 1;
            mana.Anima.choseActionByParam = (int input) =>
            {
                Transform character = Common.CharacterFouction.GetCharacter.transform;
                if (character == null) Debug.LogWarning("没有找到角色位置");
                Transform target = Common.CommonFunction.FindChildInTransform("EnemyList", character);
                Common.Handler.HandlerList.ISetOneParam<Info.CharacterInfo> set = null;
                //动画编号为0
                if (input == 0)
                {
                     set = (input) =>
                    {
                        if (input == null) return;
                        input.ChangeHP(-10, mana.CharacterInfo);
                        input.SetHardTime(0.5f);
                    };
                }
                else if (input == 1)
                {
                    set = (input) =>
                    {
                        if (input == null) return;
                        input.ChangeHP(-5, mana.CharacterInfo);
                        input.SetHardTime(0.5f);
                    };
                }

                Common.CharacterFouction.Instance.FindObjectsAndSetIt<Info.CharacterInfo>(target, 
                    //判断的函数
                    (input) =>
                {
                    if (input == null) return false;
                    Info.CharacterInfo info = input.GetComponent<Info.CharacterInfo>();
                    if (info == null) return false;
                    Vector3 targetDir = info.truthTransform.position - Control.PlayerControl.Instance.PlayerInfo.truthTransform.position;
                    if (Vector3.Dot(targetDir.normalized, Control.PlayerControl.Instance.
                        PlayerInfo.truthTransform.forward) > 0.3 &&
                        targetDir.sqrMagnitude < skillMaxRange)
                        return true;
                    return false;
                },
                //设置的函数
                    set);
            };
        }

        public override void OnCommonSkillReleasing(SkillManage mana, float nowTime)
        {
            return;
        }

        public override bool OnUnknowEndSkillReleasing(SkillManage mana)
        {
            return true;
        }
    }
}