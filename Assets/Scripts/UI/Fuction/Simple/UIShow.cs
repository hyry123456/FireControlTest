using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// һ����ֹUIһ��ʼ�͹رյ��࣬��������������
    /// </summary>
    public class UIShow : UIUseBase
    {

        protected override void Awake()
        {
            base.Awake();
            control.init += ShowSelf;
        }
    }
}