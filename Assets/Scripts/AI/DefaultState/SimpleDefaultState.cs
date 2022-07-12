
using Common.Handler;
using UnityEngine;

namespace FireControl.AI
{
    public class SimpleDefaultState : StateBase
    {

        Vector3 beginPos = Vector3.zero;

        public SimpleDefaultState()
        {
            stateType = StateType.Default;
        }

        public override void EnterState(FightAIStateMachine stateMachine, HandlerList.INonReturnAndNonParam enterFuntion)
        {
            base.EnterState(stateMachine, enterFuntion);
            if (beginPos == Vector3.zero)
            {
                beginPos = stateMachine.transform.position;
            }
        }

        public override StateBase OnFixedUpdate()
        {
            //看见目标，就进入看见目标状态
            if (stateMachine.canSeeTarget)
            {
                StateBase state = stateMachine.GetStateByType(StateType.Check);
                if (state == null)
                    return new SimpleCheckState();
                else
                    return state;
            }

            //如果不在出生点，就走到出生点
            if((stateMachine.transform.position - beginPos).sqrMagnitude > 2)
            {
                stateMachine.EnemyMotor.MoveByPos(beginPos, false);
            }
            return null;
        }

        public override void ExitState(HandlerList.INonReturnAndNonParam exitFuntion)
        {
            base.ExitState(exitFuntion);
        }
    }
}