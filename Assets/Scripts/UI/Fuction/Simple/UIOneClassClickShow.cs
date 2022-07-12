
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// 一级界面点击显示
    /// </summary>
    public class UIOneClassClickShow : UIUseBase
    {
        public string showUIName;

        protected override void Awake()
        {
            base.Awake();
            widgrt.pointerClick += ShowUI;
            control.init += ShowSelf;
        }

        private void ShowUI(PointerEventData eventData)
        {
            UICommon.Initialize.ShowUI(showUIName, control);
        }
    }
}