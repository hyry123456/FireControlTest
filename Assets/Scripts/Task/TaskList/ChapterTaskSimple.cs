
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FireControl.Task
{
    /// <summary>
    /// �½ڼ��ص�ʵ�ְ�������ʵ�����̳и���͹���
    /// </summary>
    public abstract class ChapterTaskSimple : ChapterTask
    {
        /// <summary>
        /// һ�����ڼ��ط������������
        /// </summary>
        protected string targetPart = "FireControl.Task.TaskPart";
        //��ʼ������
        //public ChapterTaskTest()
        //{
        //    chapterName = "Test";
        //    chapterTitle = "һ�������½�";
        //    taskPartSize = 2;
        //    chapterID = 0;
        //    //�����ļ��ñ������
        //    chapterSavePath = Application.streamingAssetsPath + "/" + "Task/Chapter/0.task";
        //    targetPart = targetPart + "Test";
        //}


        public override void ChangeTask()
        {
            nowCompletePart++;
            part.ExitTaskEvent(this);
            if(nowCompletePart == taskPartSize)
            {
                TaskControl.Instance.CompleteChapter(this);
                Debug.Log("�½����");
                ExitChapter();
                return;
            }
             string targetPartStr = targetPart + (nowCompletePart).ToString();
            //ʹ�÷��䴴�������ı�׼��ʽ
            Assembly assembly = Assembly.GetExecutingAssembly();
            part = (TaskPart)assembly.CreateInstance(targetPartStr);
            Debug.Log(targetPart);
            part.EnterTaskEvent(this);
            TaskControl.Instance.SaveObtainTask();
        }

        public override void CheckTask(Interaction.InteractionInfo info)
        {
            if (part.IsCompleteTask(this, info))
            {
                ChangeTask();
            }
        }

        //public override void CheckChapter()
        //{
        //    List<int> vs = TaskControl.Instance.completeTasks;
        //    if (vs == null)
        //    {
        //        LoadChapter();
        //    }
        //    else
        //    {
        //        if (!vs.Contains(chapterID))
        //        {
        //            LoadChapter();
        //        }
        //    }
        //}

        public override void BeginChapter()
        {
            string targetPartStr = targetPart + '0';
            Assembly assembly = Assembly.GetExecutingAssembly();
            part = (TaskPart)assembly.CreateInstance(targetPartStr);
            part.EnterTaskEvent(this);
        }

        public override void SetNowTaskPart(int nowPart)
        {
            nowCompletePart = nowPart;
            Assembly assembly = Assembly.GetExecutingAssembly();
            part = (TaskPart)assembly.CreateInstance(targetPart + nowPart.ToString());
            part.EnterTaskEvent(this);
        }

        //protected void LoadChapter();
        //{
        //    GameObject game = GameObject.Find("Cube (1)");
        //    InteractionInfo[] interactionInfo = game.GetComponentsInParent<InteractionInfo>();
        //    game.transform.position = this.pos;
        //    for(int i=0; i<interactionInfo.Length; i++)
        //    {
        //        GameObject.Destroy(interactionInfo[i]);
        //    }
        //    Assembly assembly = Assembly.GetExecutingAssembly();
        //    var type = assembly.GetType("FireControl.Interaction.TaskInfoTest");
        //    game.AddComponent(type);
        //}


        //public override IEnumerator ExitChapter();
    }
}