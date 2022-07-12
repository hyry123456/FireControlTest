using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// 点击后关闭UI,同时一开始显示本UI
    /// </summary>
    public class UIOnClickClose : UIUseBase
    {
        public string closeUiName;


        protected override void Awake()
        {
            base.Awake();
            widgrt.pointerClick += CloseUI;
            control.init += ShowSelf;
        }

        private void CloseUI(PointerEventData eventData)
        {
            UICommon.Initialize.CloseUI(closeUiName, control);
        }
    }
}