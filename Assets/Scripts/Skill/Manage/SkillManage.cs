
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FireControl.Skill
{
    public class SkillManage : MonoBehaviour
    {
        protected List<SkillBase> skills;
        private Info.CharacterInfo characterInfo;
        protected Anima.AnimaControl anima;
        private Motor.Motor motor;
        /// <summary>        /// 角色的移动引擎        /// </summary>
        public Motor.Motor Motor
        {
            get {
                if(motor == null)
                    motor = GetComponent<Motor.Motor>();
                return motor;
            }
        }
        public Info.CharacterInfo CharacterInfo
        {
            get
            {
                if(characterInfo == null)
                    characterInfo = GetComponentInChildren<Info.CharacterInfo>();
                return characterInfo;
            }
        }
        public Anima.AnimaControl Anima
        {
            get { 
                if(anima == null) anima = GetComponentInChildren<Anima.AnimaControl>();
                return anima; 
            }
        }

        public void CheckAndRelase(SkillBase skill)
        {
            if (skill == null) return;
            if(skill.nowCoolTime <= 0)
            {
                skill.nowCoolTime = skill.coolTime;
                StartCoroutine(SkillRelease.Instance.ReleaseSkill(this, skill));
            }
        }
        public SkillBase GetSkillByName(string skillName)
        {
            if(skills == null) return null;
            for(int i=0; i<skills.Count; i++)
            {
                if(skills[i].skillName.Equals(skillName))
                    return skills[i];
            }
            return null;
        }

        public SkillBase GetSkillByIndex(int index)
        {
            if(index > 0 && skills != null && skills.Count > index)
            {
                SkillBase skill = skills[index];
                return skill;
            }
            return null;
        }

        public List<SkillBase> GetCanUseSkill()
        {
            if (skills == null) return null;
            List<SkillBase> canUse = new List<SkillBase>(skills.Count);
            for(int i=0; i<skills.Count; i++)
            {
                if(skills[i].nowCoolTime <= 0)
                {
                    canUse.Add(skills[i]);
                }
            }
            return canUse;
        }

        /// <summary>
        /// 获得可以使用的技能，且技能类型与传入类型对应，需要注意的是支持多匹配
        /// </summary>
        /// <param name="type">技能类型</param>
        /// <returns>可以使用的技能列表，注意为空的情况</returns>
        public List<SkillBase> GetCanUseSkillByType(SkillType  type)
        {
            if (skills == null) return null;
            List<SkillBase> canUse = new List<SkillBase>();
            for(int i=0; i<skills.Count; i++)
            {
                if(skills[i].nowCoolTime <= 0 && (skills[i].skillType & type) != 0)
                {
                    canUse.Add(skills[i]);
                }
            }
            return canUse;
        }

        protected virtual void Start()
        {
            characterInfo = GetComponent<Info.CharacterInfo>();
            anima = GetComponent<Anima.AnimaControl>();
            if(characterInfo != null && characterInfo.skills != null)
            {
                skills = new List<SkillBase>();
                string targetPart = "FireControl.Skill.";
                Assembly assembly = Assembly.GetExecutingAssembly();
                for (int i=0; i<characterInfo.skills.Count; i++)
                {
                    string temp = targetPart + characterInfo.skills[i];
                    //使用反射创建类对象的标准方式
                    skills.Add((SkillBase)assembly.CreateInstance(temp));
                }
            }
        }

        protected void FixedUpdate()
        {
            if (skills == null) return;
            for(int i=0; i<skills.Count; i++)
            {
                if(skills[i].nowCoolTime > 0)
                {
                    skills[i].nowCoolTime -= Time.fixedDeltaTime;
                }
            }
        }
    }
}