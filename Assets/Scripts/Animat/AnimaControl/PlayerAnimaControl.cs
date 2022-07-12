

using UnityEngine;

namespace FireControl.Anima
{
    public class PlayerAnimaControl : AnimaControl
    {
        protected override void SetAnimaValue()
        {
            Info.PlayerAction action = characterInfo.characterAction as Info.PlayerAction;
            Animator.SetBool("Move", action.Move);
            Animator.SetBool("Jump", action.Jump);
            Animator.SetBool("Climb", action.Climb);

        }
    }
}