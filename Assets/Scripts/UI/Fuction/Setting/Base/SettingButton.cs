
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Setting
{
    /// <summary>
    /// 设置切换的点击按钮
    /// </summary>
    public class SettingButton : MonoBehaviour, IPointerClickHandler
    {
        public SettingBase Setting;
        [HideInInspector]
        public Toggle button;
        [HideInInspector]
        public SettingControl settingControl;

        public void OnPointerClick(PointerEventData eventData)
        {
            settingControl.ShowThisSetting(Setting, this);
        }

        void Start()
        {
            button = GetComponent<Toggle>();
            button.isOn = false;
        }

    }
}