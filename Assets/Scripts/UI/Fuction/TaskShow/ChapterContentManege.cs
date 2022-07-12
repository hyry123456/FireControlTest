using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// �½����ݹ����࣬������������½�����ʱ�����������ʾ��Ҳ����˵�������ʱ��Ҫ���и��½ڵ�������������ʾ
    /// ���಻���м���UIControl�й�����Ϊû�б�Ҫ��
    /// ͬʱ�޷�ȷ������ʱ�������׵�������
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
                Debug.Log("û�ж���Panel_TaskContent");
                return;
            }
            TaskContentShow taskContentShow = taskContent.GetComponent<TaskContentShow>();
            FireControl.Task.ChapterTask chapterTask = FireControl.Task.TaskControl.Instance.GetChapterByIndex(taskIndex);
            taskContentShow.ReSetContent(chapterTask.chapterName, chapterTask.GetPartName(), chapterTask.GetPartDescribe());
        }
    }
}