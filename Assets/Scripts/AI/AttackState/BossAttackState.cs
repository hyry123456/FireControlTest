
using Common.Handler;
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.AI
{

    public class BossAttackState : StateBase
    {
        public BossAttackState()
        {
            stateType = StateType.Attack;
        }


        protected bool isDodge = false;
        /// <summary>
        /// 怪物是否躲避/防御中
        /// </summary>
        public bool IsDodge
        {
            get { return isDodge; }
        }

        public override void EnterState(FightAIStateMachine stateMachine, HandlerList.INonReturnAndNonParam enterFuntion)
        {
            base.EnterState(stateMachine, enterFuntion);
            Info.BossAction selfInfo = stateMachine.EnemyInfo.characterAction as Info.BossAction;
            if (selfInfo == null)
                Debug.Log("Boss self info is null");
            else
                selfInfo.IsLock = true;
        }

        public override void ExitState(HandlerList.INonReturnAndNonParam exitFuntion)
        {
            base.ExitState(exitFuntion);
            Info.BossAction selfInfo = stateMachine.EnemyInfo.characterAction as Info.BossAction;
            if (selfInfo == null)
                Debug.Log("Boss self info is null");
            else
                selfInfo.IsLock = false;
        }

        public override StateBase OnFixedUpdate()
        {
            //目标信息
            Info.CharacterInfo targetInfo = stateMachine.GetTarget;
            if (targetInfo == null)
                return stateMachine.GetStateByType(StateType.Check);

            Info.BossAction selfInfo = stateMachine.EnemyInfo.characterAction as Info.BossAction;
            if (selfInfo == null)
                Debug.LogError("Boss self info is null");
            //selfInfo.Move = 0;

            Skill.EnemySkillMange skillMange = stateMachine.SkillManage as Skill.EnemySkillMange;
            if (skillMange == null)
            {
                Debug.LogError("怪物没有挂怪物的技能管理类");
                return stateMachine.GetStateByType(StateType.Check);
            }

            List<Skill.SkillInfo> skillInfos = skillMange.GetSkillInfoByRange(stateMachine.targetSqrDistance);

            //一般是太远了
            if (skillInfos == null || skillInfos.Count == 0)
            {
                //实际上挂的是Boss的移动引擎，用来旋转而已
                stateMachine.EnemyMotor.MoveByDir(stateMachine.targetDir, true);
                //selfInfo.Move = 1;
                return null;
            }

            for(int i=0; i<skillInfos.Count; i++)
            {
                if (skillInfos[i].canUse)
                {
                    skillMange.CheckAndRelase(skillInfos[i].skill);
                    skillInfos.Clear();
                    skillInfos = null;
                    return null;
                }
            }

            Skill.SkillBase rollSkill = Common.CommonFunction.ChoseOneOnList(
                skillMange.GetCanUseSkillByType(Skill.SkillType.Dodge));


            skillMange.CheckAndRelase(rollSkill);
            skillInfos.Clear();
            skillInfos = null;
            return null;

        }
    }
}