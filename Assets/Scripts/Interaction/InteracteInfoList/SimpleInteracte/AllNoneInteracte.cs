
using System.Collections;


namespace FireControl.Interaction
{
    /// <summary>
    /// 一个全都是拓展的交互类，一般用来由交互控制执行一些简单的行为
    /// 需要注意的是这个交互调用不了任务行为，因为这个类没有继承任务交互信息，无法在Chapter中正常检测
    /// </summary>
    public class AllNoneInteracte : InteractionInfo
    {
        private string addTaskName;
        /// <summary>
        /// 全都是空的交互案例，存储着一些行为，用来作为其交互行为
        /// </summary>
        /// <param name="type">交互类型</param>
        /// <param name="runFunction">运行方法</param>
        public AllNoneInteracte(InteractionType type,
            Common.Handler.HandlerList.INonReturnAndNonParam runFunction)
        {
            nonReAndNonParam = runFunction;
            this.interactionType = type;
        }
        /// <summary>        /// 一个补充事件，用来对该方法进行一些简单拓展        /// </summary>
        private Common.Handler.HandlerList.INonReturnAndNonParam nonReAndNonParam;
        public override IEnumerator InteractionBehavior(InteractionControl interControl)
        {
            if(nonReAndNonParam != null)
            {
                nonReAndNonParam();
            }
            yield break;
        }

        protected override void OnEnable()
        {
            return;
        }
    }
}