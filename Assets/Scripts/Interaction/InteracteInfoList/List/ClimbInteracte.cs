using System.Collections;
using UnityEngine;

namespace FireControl.Interaction
{

    public class ClimbInteracte : InteractionInfo
    {

        public override IEnumerator InteractionBehavior(InteractionControl interControl)
        {
            interControl.StartInteraction();
            interControl.PlayerInfo.allSystemPort.PlayerMotor.moveSpeed = Vector3.zero;
            interControl.PlayerInfo.allSystemPort.AnimaControl.StartRootAnimate();
            yield return new WaitForSeconds(2f);
            interControl.PlayerInfo.allSystemPort.AnimaControl.StopRootAnimate();
            interControl.StopInteraction();
        }

        protected override void OnEnable()
        {
            interactionType = InteractionType.Move;
            interactionName = "ClimbInteracte";
        }
    }
}