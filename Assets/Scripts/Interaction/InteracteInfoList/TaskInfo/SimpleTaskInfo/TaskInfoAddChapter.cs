
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace FireControl.Interaction
{
    /// <summary>
    /// ��׼��������¼���������������½ڣ�Ҳ���Ǽ򵥵������½�
    /// </summary>
    public class TaskInfoAddChapter : InteracteTaskInfo
    {
        public string addTaskName;
        /// <summary>        /// һ�������¼��������Ը÷�������һЩ����չ        /// </summary>
        public Common.Handler.HandlerList.INonReturnAndNonParam nonReAndNonParam;
        
        public override IEnumerator InteractionBehavior(InteractionControl interControl)
        {
            //ʹ�÷��䴴�������ı�׼��ʽ
            Assembly assembly = Assembly.GetExecutingAssembly();
            Task.ChapterTask chapter = (Task.ChapterTask)assembly.CreateInstance(addTaskName);
            if(chapter == null)
            {
                Debug.LogError("����");
            }
            Task.TaskControl.Instance.AddChapter(chapter);
            //��չ������Ϊ
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