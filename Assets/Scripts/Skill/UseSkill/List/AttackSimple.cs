
using UnityEngine;


namespace FireControl.Skill
{
    /// <summary>
    /// �����õĹ�������
    /// </summary>
    public class AttackSimple : SkillBase
    {
        public AttackSimple()
        {
            skillType = SkillType.NearDisAttack;
            expendSP = 0;
            coolTime = 2f;
            relaseTime = 2f;
            skillName = "����";
            skillMaxRange = 4f;
        }

        public override void OnSkillEnd(SkillManage mana)
        {
            //mana.CharacterInfo.characterAction.Attack = 0;

        }

        public override void OnSkillRelease(SkillManage mana)
        {
            Debug.Log("��������");
            if (mana.Anima == null) Debug.LogWarning("û�ж���");
            //mana.CharacterInfo.characterAction.Attack = 1;
            mana.Anima.choseActionByParam = (int i) =>
            {
                if ((Control.PlayerControl.Instance.transform.position - mana.transform.position).sqrMagnitude <
                    skillMaxRange)
                {
                    Info.CharacterInfo info = Control.PlayerControl.Instance.GetComponent<Info.CharacterInfo>();
                    Control.PlayerControl.Instance.GetComponent<Info.CharacterInfo>().ChangeHP(-10, mana.CharacterInfo);
                }
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