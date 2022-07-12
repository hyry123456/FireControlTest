
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace FireControl.Interaction
{
    /// <summary>
    /// 标准添加任务事件，用来添加任务章节，也就是简单的启动章节
    /// </summary>
    public class TaskInfoAddChapter : InteracteTaskInfo
    {
        public string addTaskName;
        /// <summary>        /// 一个补充事件，用来对该方法进行一些简单拓展        /// </summary>
        public Common.Handler.HandlerList.INonReturnAndNonParam nonReAndNonParam;
        
        public override IEnumerator InteractionBehavior(InteractionControl interControl)
        {
            //使用反射创建类对象的标准方式
            Assembly assembly = Assembly.GetExecutingAssembly();
            Task.ChapterTask chapter = (Task.ChapterTask)assembly.CreateInstance(addTaskName);
            if(chapter == null)
            {
                Debug.LogError("空了");
            }
            Task.TaskControl.Instance.AddChapter(chapter);
            //拓展其他行为
            if (nonReAndNonParam != null)
                nonReAndNonParam();
            yield break;
        }

        protected override void OnEnable()
        {
            interactionType = InteractionType.Task;
            interactionID = 1;
        }

    }
}