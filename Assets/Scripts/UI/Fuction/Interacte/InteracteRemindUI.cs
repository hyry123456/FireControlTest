
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// ������ʾUI�Ĺ����࣬������ǹ��ڸ������ϵ�
    /// </summary>
    public class InteracteRemindUI : UIUseBase
    {

        Text interacteRemindText;
        string intercateString;

        protected override void Awake()
        {
            base.Awake();
            interacteRemindText = GetComponentInChildren<Text>();
            Common.ResetInput.MyInput myInput = Common.ResetInput.MyInput.Instance;
            Common.ResetInput.MyInputValueStruct myInputValueStruct;
            if (myInput.GetAsxisStruct("Interaction", out myInputValueStruct))
            {
                intercateString = "����" + myInputValueStruct.valueUp;
            }
            else
                Debug.Log("û�н�������");
        }

        /// <summary>
        /// ��ʾ��ǰ������Ϣ
        /// </summary>
        /// <param name="interactionType">���뽻������</param>
        public void SetAndShowReminderText(FireControl.Interaction.InteractionType interactionType)
        {
            switch (interactionType)
            {
                case FireControl.Interaction.InteractionType.PasserBy:
                case FireControl.Interaction.InteractionType.Task:
                    gameObject.SetActive(true);
                    interacteRemindText.text = intercateString + "���жԻ�";
                    break;
                case FireControl.Interaction.InteractionType.Object:
                    gameObject.SetActive(true);
                    interacteRemindText.text = intercateString + "ʰȡ����";
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            interacteRemindText.text = "";
        }

    }
}