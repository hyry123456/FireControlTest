
using System.Collections;
using UnityEngine;

namespace FireControl.Interaction
{
    public class TestInteraction : InteractionInfo
    {
        protected override void OnEnable()
        {
            this.interactionType = InteractionType.PasserBy;
            this.interactionID = 0;
        }
        public override IEnumerator InteractionBehavior(InteractionControl interControl)
        {
            Debug.Log("·¢Éú½»»¥");
            yield break;
        }

    }
}
