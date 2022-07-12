using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Skill
{
    /// <summary>
    /// ������Ϣ�ṹ�壬�����洢������ܵ�������ܷ�ʹ�õ�
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
    /// AI�õļ��ܹ����࣬������װһЩ�����ڹ����õķ���
    /// </summary>
    public class EnemySkillMange : SkillManage
    {

        /// <summary>
        /// ��ȡ���з��ϸ÷�Χ�ļ��ܵļ�����Ϣ
        /// </summary>
        /// <param name="sqrRange">�����ķ�Χ��ƽ������Ϊ���Ҳ����ƽ������</param>
        /// <returns>��Ӧ�ļ�����Ϣ����</returns>
        public List<SkillInfo> GetSkillInfoByRange(float sqrRange)
        {
            if(skills == null || skills.Count == 0) return null;
            List<SkillInfo> skillInfoList = new List<SkillInfo>();
            for(int i=0; i<skills.Count; i++)
            {
                //�ڷ�Χ�Ĳ�����
                if(sqrRange <= skills[i].skillMaxRange && sqrRange >= skills[i].skillMinRange)
                {
                    //��ֵ�ڲ��Լ���������
                    skillInfoList.Add(new SkillInfo(skills[i], (skills[i].nowCoolTime <= 0)));
                }
            }
            return skillInfoList;
        }
    }
}