using Common.Handler;
using UnityEngine;


namespace FireControl.AI
{
    /// <summary>
    /// 测试用的默认状态机
    /// </summary>
    public class SimpleCheckState : StateBase
    {
        /// <summary>        /// 发现的数值，用来判断是否发现目标        /// </summary>
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
            //赋值一下1，可能主角会跑出范围，所以锁定值需要有一定的保存
            findSize = 0.9f;
            return;
        }

        public override StateBase OnFixedUpdate()
        {
            //没有目标，逐渐下降，0.1是减低速度
            if (!stateMachine.canSeeTarget) {
                findSize -= Time.fixedDeltaTime ;
            }
            //看得到，开始上升
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

            //发现就逐渐看向主角
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