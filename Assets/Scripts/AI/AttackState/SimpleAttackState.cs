using Common.Handler;
using System.Collections.Generic;


namespace FireControl.AI
{
    /// <summary>    /// ����ģʽ���洢�ŵ�ǰĿ��ľ���ģʽ���ж�AI��Ϊ    /// </summary>
    enum DistanceMode
    {
        /// <summary>        /// ׷����Ϊ        /// </summary>
        Pursue,
        /// <summary>        /// ������Ϊ        /// </summary>
        Attack,
    }
    
    /// <summary>
    /// �����ĵ��˹���AI����������Ŀ��,���AI�ǽ�սС����AI��ֻ�м򵥵�����
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
            //ֻ�е����㹻����Ŀ��ʱ�Ž��й���
            if(stateMachine.targetSqrDistance < stateMachine.EnemyInfo.attackSqrDis - 1)
                distanceMode = DistanceMode.Attack;
            else
                distanceMode = DistanceMode.Pursue;
        }

        public override StateBase OnFixedUpdate()
        {
            //���ɿ���Ŀ��
            if (!stateMachine.canSeeTarget)
            {
                //�������룬�˳�����״̬
                if(stateMachine.targetSqrDistance > stateMachine.EnemyInfo.seeSqrDistance)
                {
                    StateBase state = stateMachine.GetStateByType(StateType.Check);
                    if(state != null)
                        return state;
                    else return new SimpleCheckState();
                }
                //׷������
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