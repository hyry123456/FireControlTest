
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 交互提示UI的管理类，这个类是挂在父物体上的
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
                intercateString = "按下" + myInputValueStruct.valueUp;
            }
            else
                Debug.Log("没有交互按键");
        }

        /// <summary>
        /// 显示当前交互信息
        /// </summary>
        /// <param name="interactionType">传入交互类型</param>
        public void SetAndShowReminderText(FireControl.Interaction.InteractionType interactionType)
        {
            switch (interactionType)
            {
                case FireControl.Interaction.InteractionType.PasserBy:
                case FireControl.Interaction.InteractionType.Task:
                    gameObject.SetActive(true);
                    interacteRemindText.text = intercateString + "进行对话";
                    break;
                case FireControl.Interaction.InteractionType.Object:
                    gameObject.SetActive(true);
                    interacteRemindText.text = intercateString + "拾取物体";
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