using System.Collections;

namespace FireControl.Interaction
{
    /// <summary>
    /// 任务交互的检测案例，对于一般的任务，只需要调用该类，然后设置一下ID值就行了
    /// </summary>
    public class TaskInfoCheck : InteracteTaskInfo
    {
        //private string addTaskName;
        /// <summary>        /// 一个补充事件，用来对该方法进行一些简单拓展        /// </summary>
        public Common.Handler.HandlerList.INonReturnAndNonParam nonReAndNonParam;

        public TaskInfoCheck(int chapterID)
        {
            this.chapterID = chapterID;
        }

        public override IEnumerator InteractionBehavior(InteractionControl interControl)
        {
            Task.TaskControl.Instance.CheckChapter(this);
            if (nonReAndNonParam != null)
                nonReAndNonParam();
            yield break;
        }

        protected override void OnEnable()
        {
            interactionType = InteractionType.Task;
        }

        /// <summary>
        /// 由于设置交互事件为物体的交互事件时会使用AddComponent来代替跳过构造函数创建对象
        /// 所以使用这个方法来设置章节ID
        /// </summary>
        public void SetChapterID(int id)
        {
            chapterID = id;
        }
    }
}