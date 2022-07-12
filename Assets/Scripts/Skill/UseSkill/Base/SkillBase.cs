
namespace FireControl.Skill
{
    public abstract class SkillBase : ISkill
    {
        public abstract void OnSkillEnd(SkillManage mana);

        public abstract void OnSkillRelease(SkillManage mana);

        /// <summary>
        /// �ڼ����ͷ���ʱʱ���õķ�������������һЩ�˶���Ϊ
        /// </summary>
        /// <param name="mana">���ܹ������</param>
        /// <param name="nowTime">��ǰ�ļ����ͷ�ʱ�䣬�ӱ����release��ʼ���𽥱�Ϊ0</param>
        public abstract void OnCommonSkillReleasing(SkillManage mana, float nowTime);

        /// <summary>
        /// �ڲ�ȷ������ʱ��ʱ��������֡���õķ�������������ֵȷ������
        /// ���ͷ�ʱ��Ϊ����ʱ��ִ�и÷�����Ҳ���ǲ�ȷ���ͷ�ʱ��ʱ
        /// </summary>
        /// <param name="mana">���ܹ������</param>
        /// <returns>�Ƿ����,true������false������</returns>
        public abstract bool OnUnknowEndSkillReleasing(SkillManage mana);

        public SkillType skillType;
        public int expendSP;
        /// <summary>        /// ��ǰ��ȴʱ�䣬���������ܿ������жϼ����ܲ����ͷ�        /// </summary>
        public float nowCoolTime;
        /// <summary>        /// ������ȴʱ�䣬��ȴʱ��û�н���������ֹͣ����        /// </summary>
        public float coolTime;
        /// <summary>        /// ���ܵ��ͷ�ʱ�䣬�ͷż��ܺ󾭹���ʱ�����ü��ܽ���        /// </summary>
        public float relaseTime;
        /// <summary>        /// ��������        /// </summary>
        public string skillName;
        /// <summary>        /// �������Χ        /// </summary>
        public float skillMaxRange;
        /// <summary>        /// ������С��Χ�����������ǹ��Ƚ���ʱ������ת�ļ���        /// </summary>
        public float skillMinRange;
    }
}