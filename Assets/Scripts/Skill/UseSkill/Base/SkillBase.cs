
namespace FireControl.Skill
{
    public abstract class SkillBase : ISkill
    {
        public abstract void OnSkillEnd(SkillManage mana);

        public abstract void OnSkillRelease(SkillManage mana);

        /// <summary>
        /// 在技能释放中时时调用的方法，用来进行一些运动行为
        /// </summary>
        /// <param name="mana">技能管理组件</param>
        /// <param name="nowTime">当前的技能释放时间，从本类的release开始，逐渐变为0</param>
        public abstract void OnCommonSkillReleasing(SkillManage mana, float nowTime);

        /// <summary>
        /// 在不确定结束时间时，逐物理帧调用的方法，依靠返回值确定结束
        /// 但释放时间为负数时会执行该方法，也就是不确定释放时间时
        /// </summary>
        /// <param name="mana">技能管理组件</param>
        /// <returns>是否结束,true结束，false不结束</returns>
        public abstract bool OnUnknowEndSkillReleasing(SkillManage mana);

        public SkillType skillType;
        public int expendSP;
        /// <summary>        /// 当前冷却时间，用来给技能控制器判断技能能不能释放        /// </summary>
        public float nowCoolTime;
        /// <summary>        /// 技能冷却时间，冷却时间没有结束，不能停止技能        /// </summary>
        public float coolTime;
        /// <summary>        /// 技能的释放时间，释放技能后经过该时间会调用技能结束        /// </summary>
        public float relaseTime;
        /// <summary>        /// 技能名称        /// </summary>
        public string skillName;
        /// <summary>        /// 技能最大范围        /// </summary>
        public float skillMaxRange;
        /// <summary>        /// 技能最小范围，用来当主角过度近身时限制旋转的技能        /// </summary>
        public float skillMinRange;
    }
}