using Common.Handler;
using UnityEngine;


namespace FireControl.AI
{
    /// <summary>
    /// �����õ�Ĭ��״̬��
    /// </summary>
    public class SimpleCheckState : StateBase
    {
        /// <summary>        /// ���ֵ���ֵ�������ж��Ƿ���Ŀ��        /// </summary>
        float findSize = 0;
        public SimpleCheckState()
        {
            stateType = StateType.Check;
        }

        public override void EnterState(FightAIStateMachine stateMachine, HandlerList.INonReturnAndNonParam enterFuntion)
        {
            base.EnterState(stateMachine, enterFuntion);
        }

        public override void ExitState(HandlerList.INonReturnAndNonParam exitFuntion)
        {
            base.ExitState(exitFuntion);
            //��ֵһ��1���������ǻ��ܳ���Χ����������ֵ��Ҫ��һ���ı���
            findSize = 0.9f;
            return;
        }

        public override StateBase OnFixedUpdate()
        {
            //û��Ŀ�꣬���½���0.1�Ǽ����ٶ�
            if (!stateMachine.canSeeTarget) {
                findSize -= Time.fixedDeltaTime ;
            }
            //���õ�����ʼ����
            else
            {
                findSize += stateMachine.targetSqrDistance / stateMachine.EnemyInfo.seeSqrDistance * Time.fixedDeltaTime;
            }
            
            if(findSize <= 0)
            {
                StateBase state = stateMachine.GetStateByType(StateType.Default);
                if(state == null)
                    return new SimpleDefaultState();
                else
                    return state;
            }

            //���־��𽥿�������
            if(findSize > 0)
                stateMachine.EnemyMotor.LookByVector(stateMachine.targetDir);
            if (findSize > 1)
            {
                StateBase stateBase = stateMachine.GetStateByType(StateType.Attack);
                if(stateBase != null)
                    return stateBase;
                else
                    return new SimpleAttackState();
            }

            stateMachine.EnemyMotor.MoveByDir(stateMachine.targetDir, false);
            
            return null;
        }
    }

}