using Common.Handler;
using System.Collections.Generic;


namespace FireControl.AI
{
    /// <summary>    /// 距离模式，存储着当前目标的距离模式，判断AI行为    /// </summary>
    enum DistanceMode
    {
        /// <summary>        /// 追赶行为        /// </summary>
        Pursue,
        /// <summary>        /// 攻击行为        /// </summary>
        Attack,
    }
    
    /// <summary>
    /// 案例的敌人攻击AI，用来攻击目标,这个AI是近战小兵的AI，只有简单的智能
    /// </summary>
    public class SimpleAttackState : StateBase
    {

        public SimpleAttackState()
        {
            stateType = StateType.Attack;
        }

        private DistanceMode distanceMode;

        private void LoadDistanceMode()
        {
            //只有敌人足够靠近目标时才进行攻击
            if(stateMachine.targetSqrDistance < stateMachine.EnemyInfo.attackSqrDis - 1)
                distanceMode = DistanceMode.Attack;
            else
                distanceMode = DistanceMode.Pursue;
        }

        public override StateBase OnFixedUpdate()
        {
            //不可看见目标
            if (!stateMachine.canSeeTarget)
            {
                //超出距离，退出攻击状态
                if(stateMachine.targetSqrDistance > stateMachine.EnemyInfo.seeSqrDistance)
                {
                    StateBase state = stateMachine.GetStateByType(StateType.Check);
                    if(state != null)
                        return state;
                    else return new SimpleCheckState();
                }
                //追踪主角
                stateMachine.EnemyMotor.MoveByDir(stateMachine.targetDir, true);
                return null;
            }
            LoadDistanceMode();
            switch (distanceMode)
            {
                case DistanceMode.Pursue:
                    stateMachine.EnemyMotor.MoveByDir(stateMachine.targetDir, true);
                    break;
                case DistanceMode.Attack:
                    List<Skill.SkillBase> skillBases = 
                        stateMachine.SkillManage.GetCanUseSkillByType(Skill.SkillType.NearDisAttack);
                    if (skillBases == null)
                        break;
                    else if(skillBases.Count== 0)
                    {
                        skillBases.Clear();
                        skillBases = null;
                        break;
                    }
                    else
                    {
                        stateMachine.SkillManage.CheckAndRelase(
                            Common.CommonFunction.ChoseOneOnList(skillBases));
                        skillBases.Clear();
                        skillBases = null;
                        break;
                    }
                default:
                    break;
            }
            return null;
        }

        public override void EnterState(FightAIStateMachine stateMachine, HandlerList.INonReturnAndNonParam enterFuntion)
        {
            base.EnterState(stateMachine, enterFuntion);
        }
        public override void ExitState(HandlerList.INonReturnAndNonParam exitFuntion)
        {
            base.ExitState(exitFuntion);
        }
    }
}