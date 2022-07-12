using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// ��ʾ���������ı��࣬ͨ��ʹ��UiControl��ȡ���������
    /// ���������������ݵ���ʾ
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
        /// �����������ʾ����
        /// </summary>
        /// <param name="chapter">�½�����</param>
        /// <param name="taskPart">С������</param>
        /// <param name="descipe">С������</param>
        public void ReSetContent(string chapter, string taskPart, string descipe)
        {
            chapterName.text = chapter;
            taskPartName.text = taskPart;
            taskPartDescripe.text = descipe;
        }
    }
}