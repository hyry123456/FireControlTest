using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// 章节内容管理类，用来当点击该章节名称时对任务进行显示，也就是说当被点击时需要进行该章节的内容描述的显示
    /// 该类不进行加入UIControl中管理，因为没有必要，
    /// 同时无法确定改名时机，容易导致重名
    /// </summary>
    public class ChapterContentManege : UIUseBase
    {
        public int taskIndex = 0;

        protected override void Awake()
        {
            base.Awake();
            widgrt.pointerClick += OnClickShowContext;
        }
        private void OnClickShowContext(PointerEventData eventData)
        {
            GameObject taskContent = UICommon.Initialize.GetGameObject("Panel_TaskContent", control);
            if(taskContent == null)
            {
                Debug.Log("没有对象Panel_TaskContent");
                return;
            }
            TaskContentShow taskContentShow = taskContent.GetComponent<TaskContentShow>();
            FireControl.Task.ChapterTask chapterTask = FireControl.Task.TaskControl.Instance.GetChapterByIndex(taskIndex);
            taskContentShow.ReSetContent(chapterTask.chapterName, chapterTask.GetPartName(), chapterTask.GetPartDescribe());
        }
    }
}