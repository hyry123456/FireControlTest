
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Setting
{
    /// <summary>
    /// �����л��ĵ����ť
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