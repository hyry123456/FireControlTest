using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Skill
{
    /// <summary>
    /// 技能信息结构体，用来存储这个技能的情况，能否使用等
    /// </summary>
    public struct SkillInfo
    {
        public SkillBase skill;
        public bool canUse;
        public SkillInfo(SkillBase skill, bool canUse)
        {
            this.skill = skill;
            this.canUse = canUse;
        }
    }



    /// <summary>
    /// AI用的技能管理类，用来封装一些方便于怪物用的方法
    /// </summary>
    public class EnemySkillMange : SkillManage
    {

        /// <summary>
        /// 获取所有符合该范围的技能的技能信息
        /// </summary>
        /// <param name="sqrRange">搜索的范围的平方，因为检测也是用平方检测的</param>
        /// <returns>对应的技能信息数组</returns>
        public List<SkillInfo> GetSkillInfoByRange(float sqrRange)
        {
            if(skills == null || skills.Count == 0) return null;
            List<SkillInfo> skillInfoList = new List<SkillInfo>();
            for(int i=0; i<skills.Count; i++)
            {
                //在范围的才塞入
                if(sqrRange <= skills[i].skillMaxRange && sqrRange >= skills[i].skillMinRange)
                {
                    //赋值内部以及插入数据
                    skillInfoList.Add(new SkillInfo(skills[i], (skills[i].nowCoolTime <= 0)));
                }
            }
            return skillInfoList;
        }
    }
}