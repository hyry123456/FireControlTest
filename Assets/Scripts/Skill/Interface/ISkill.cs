
namespace FireControl.Skill {
    public interface ISkill
    {
        void OnSkillRelease(SkillManage mana);
        void OnSkillEnd(SkillManage mana);
    }
}