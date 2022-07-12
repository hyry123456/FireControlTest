using System.Collections;
using UnityEngine;

namespace FireControl.Interaction
{
    public abstract class InteractionInfo : MonoBehaviour
    {
        [HideInInspector]
        public InteractionType interactionType;
        /// <summary>        /// 需要注意，交互ID一般不是用于特别指定，而是在一些特殊情况下进行临时赋值用的
        /// 也就是说初始化不需要赋值该属性/// </summary>
        public int interactionID = 0;
        /// <summary>        /// 初始化类名称，ID赋值太麻烦了，不好排序        /// </summary>
        public string interactionName;

        /// <summary>
        /// 初始化InteractionType还有interactionName
        /// </summary>
        protected abstract void OnEnable();

        /// <summary>
        /// 该交互行为需要干的事情
        /// </summary>
        /// <param name="interControl">交互管理组件</param>
        /// <returns>协程放回，方便延时</returns>
        public abstract IEnumerator InteractionBehavior(InteractionControl interControl);

    }
}