using Common.Handler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.AI
{
    public class BaoGuoCheckState : StateBase
    {
        /// <summary>        /// 发现的数值，用来判断是否发现目标        /// </summary>
        float findSize = 0;
        public BaoGuoCheckState()
        {
            stateType = StateType.Check;
        }

        public override void EnterState(FightAIStateMachine stateMachine, HandlerList.INonReturnAndNonParam enterFuntion)
        {
            Debug.Log("保国进入检查");
            base.EnterState(stateMachine, enterFuntion);

        }

        public override void ExitState(HandlerList.INonReturnAndNonParam exitFuntion)
        {
            base.ExitState(exitFuntion);
            findSize = 0.9f;
            return;
        }

        public override StateBase OnFixedUpdate()
        {
            //确定是否要退出
            if (findSize <= 0)
            {
                StateBase state = stateMachine.GetStateByType(StateType.Default);
                if (state == null)
                    return new SimpleDefaultState();
                else
                    return state;
            }

            //获得信息存储变量
            Info.BossAction bossAction = stateMachine.EnemyInfo.characterAction as Info.BossAction;
            //没有目标，逐渐下降，0.1是减低速度
            if (!stateMachine.canSeeTarget)
            {
                findSize -= Time.fixedDeltaTime;
                //停止移动
                //if (bossAction != null)
                //    bossAction.Move = 0;
                //else Debug.Log("BossAction is null");
                return null;
            }
            //看得到，开始上升
            else
            {
                findSize += stateMachine.targetSqrDistance / stateMachine.EnemyInfo.seeSqrDistance * Time.fixedDeltaTime;
            }

            //发现就逐渐看向主角
            if (findSize > 0)
                stateMachine.EnemyMotor.LookByVector(stateMachine.targetDir);
            if (findSize > 1)
            {
                StateBase stateBase = stateMachine.GetStateByType(StateType.Attack);
                if (stateBase != null)
                    return stateBase;
                else
                    return new SimpleAttackState();
            }

            //if (bossAction != null)
            //    bossAction.Move = 1;
            else Debug.Log("BossAction is null");
            return null;
        }
    }
}