
using UnityEngine;


namespace UI.Setting
{
    /// <summary>
    /// 设置显示的管理组件
    /// </summary>
    public class SettingControl : UIUseBase
    {
        private SettingBase nowSetting;
        private SettingButton nowSettingButton;
        private bool isBegin;
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            SettingButton[] buttons = transform.Find("Panel_ButtonList").GetComponentsInChildren<SettingButton>();
            for(int i=0; i<buttons.Length; i++)
            {
                buttons[i].settingControl = this;
            }
        }

        private void OnEnable()
        {
            isBegin = true;
        }

        public void Update()
        {
            if (isBegin)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    FireControl.Interaction.InteractionControl.Instance.StopInteraction();
                    gameObject.SetActive(false);
                }
            }
        }

        public void ShowSetting()
        {
            gameObject.SetActive(true);
            FireControl.Interaction.InteractionControl.Instance.StartInteraction();
            SettingButton button = transform.Find("Panel_ButtonList").GetComponentInChildren<SettingButton>();

            button.OnPointerClick(null);
        }

        public void ShowThisSetting(SettingBase setting, SettingButton button)
        {
            if (nowSetting == setting)
            {
                return;
            }
            if (nowSetting != null)
            {
                nowSetting.gameObject.SetActive(false);
            }
            setting.BeginSetting();
            nowSetting = setting;
            if(nowSettingButton != null)
            {
                nowSettingButton.button.Select();
                nowSettingButton.button.isOn = false;
            }
            nowSettingButton = button;
        }

        private void OnDisable()
        {
            nowSetting = null;
            nowSettingButton = null;
            SettingBase[] settingBases = GetComponentsInChildren<SettingBase>();
            for(int i=0; i< settingBases.Length; i++)
            {
                settingBases[i].EndSetting();
            }
            isBegin = false;
        }

    }
}