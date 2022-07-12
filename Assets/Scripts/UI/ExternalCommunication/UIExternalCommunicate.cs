
using UnityEngine;

namespace UI
{
    /// <summary>
    /// UI系统对外接口，UIControl干的东西太单一了，不适合直接外界调用
    /// 外界调用要获取uiControl又要获取UICommon太麻烦，所以直接用这个类
    /// 将行为封装起来，方便使用，需要注意的是UI内部不
    /// </summary>
    public class UIExternalCommunicate : MonoBehaviour
    {

        /// <summary>
        /// 用于获取这个场景的UI管理员
        /// </summary>
        protected UIControl control;
        protected virtual void Start()
        {
            control = GetComponent<UIControl>();
        }


        /// <summary>
        /// 设置UI显示
        /// </summary>
        /// <param name="uiName">显示的UI名称</param>
        public void SetUIActive(string uiName)
        {
            if (control == null) return;
            UICommon.Initialize.ShowUI(uiName, control);
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiName">关闭的UI名称</param>
        public void CloseUI(string uiName)
        {
            UICommon.Initialize.CloseUI(uiName, control);
        }

    }
}