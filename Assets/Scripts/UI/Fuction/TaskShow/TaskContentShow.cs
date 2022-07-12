using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 显示任务内容文本类，通常使用UiControl获取对象后设置
    /// 用来进行任务内容的显示
    /// </summary>
    public class TaskContentShow : UIUseBase
    {
        private Text chapterName;
        private Text taskPartName;
        private Text taskPartDescripe;

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            chapterName = transform.Find("Text_ChapterName").GetComponent<Text>();
            taskPartDescripe = transform.Find("Text_TaskDecipe").GetComponent<Text>();
            taskPartName = transform.Find("Text_TaskName").GetComponent <Text>();
        }

        /// <summary>
        /// 设置任务的显示内容
        /// </summary>
        /// <param name="chapter">章节名称</param>
        /// <param name="taskPart">小节名称</param>
        /// <param name="descipe">小节描述</param>
        public void ReSetContent(string chapter, string taskPart, string descipe)
        {
            chapterName.text = chapter;
            taskPartName.text = taskPart;
            taskPartDescripe.text = descipe;
        }
    }
}