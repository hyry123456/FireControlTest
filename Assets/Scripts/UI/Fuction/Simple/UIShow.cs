using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// 一个防止UI一开始就关闭的类，不进行其他操作
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