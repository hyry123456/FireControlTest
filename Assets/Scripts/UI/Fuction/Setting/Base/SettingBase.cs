
using UnityEngine;

namespace UI.Setting
{
    public abstract class SettingBase : UIUseBase
    {
        protected bool isSetting;
        protected override void Awake()
        {
            base.Awake();
            isSetting = false;
        }

        /// <summary>
        /// Enable也设置为false，保证不会乱开启，只有BeginSetting才会开启
        /// </summary>
        private void OnEnable()
        {
            isSetting = false;   
        }

        /// <summary>
        /// 关闭显示时，必须标识这个的设置已经停止
        /// </summary>
        private void OnDisable()
        {
            isSetting = false;
        }

        /// <summary>
        /// 开启了这个设置界面，启动这个设置界面的行为
        /// </summary>
        public abstract void BeginSetting();

        /// <summary>
        /// 停止这个事件，这个一半会在事件管理退出后执行，切换时不会执行
        /// </summary>
        public abstract void EndSetting();
    }
}