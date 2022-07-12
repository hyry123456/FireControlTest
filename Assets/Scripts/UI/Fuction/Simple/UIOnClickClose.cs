using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// �����ر�UI,ͬʱһ��ʼ��ʾ��UI
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