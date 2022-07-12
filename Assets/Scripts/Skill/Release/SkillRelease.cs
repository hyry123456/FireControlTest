using System.Collections;
using UnityEngine;

namespace FireControl.Skill
{
    public class SkillRelease : MonoBehaviour
    {
        private static SkillRelease release;
        //private SkillRelease()
        //{
        //}

        public static SkillRelease Instance
        {
            get
            {
                if (release == null)
                {
                    GameObject go = new GameObject("SkillRelease");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    release = go.AddComponent<SkillRelease>();
                }
                return release;
            }
        }

        public IEnumerator ReleaseSkill(SkillManage mana, SkillBase skill)
        {
            mana.CharacterInfo.SetHardTime(skill.relaseTime);
            skill.OnSkillRelease(mana);
            float releaseTime = skill.relaseTime;
            //ʱ��δ֪
            if(releaseTime < 0)
                while(!skill.OnUnknowEndSkillReleasing(mana))
                    yield return new WaitForFixedUpdate();
            //ʱ����֪
            else
            {
                while (releaseTime > 0)
                {
                    yield return new WaitForFixedUpdate();
                    skill.OnCommonSkillReleasing(mana, releaseTime);
                    releaseTime -= Time.fixedDeltaTime;
                }
            }
            //����֡ʱʱ����
            skill.OnSkillEnd(mana);
        }

    }
}