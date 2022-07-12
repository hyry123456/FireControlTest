

using System;
using UnityEngine;

namespace FireControl.Task
{
    public abstract class TaskPart
    {
        public string partName;
        public string partDescribe;

        /// <summary>
        /// 进入任务事件，该函数有三种情况调用，一种是正常手段加载任务调用，
        /// 也就是由TaskControl调用BeginChapter进行第一次加载场景时调用，
        /// 第二种是由已经获取的任务中进行文本文件读取时加载任务的SetNowTaskPart进行调用
        /// 还有就是正常的章节切换时进入该章节的任务
        /// </summary>
        /// <param name="chapter">章节名称</param>
        public abstract void EnterTaskEvent(ChapterTask chapter);
        /// <summary>
        /// 是否任务完成任务，用于检测任务是否完成，一般只有在传入一个任务交互，同时该交互的对应章节ID
        /// 就是该任务时才会进行使用，一般不用担心是否会交互出错，也就是被其他任务调用了
        /// </summary>
        /// <param name="chapter">章节名称</param>
        /// <param name="interactionInfo">交互信息</param>
        /// <returns>是否完成任务</returns>
        public abstract bool IsCompleteTask(ChapterTask chapter, Interaction.InteractionInfo interactionInfo);
        /// <summary>
        /// 章节退出事件，由IsCompleteTask返回任务完成(true)后由ChangeTask调用，
        /// 用来表示该小节结束，进入下一小节
        /// </summary>
        /// <param name="chapter"></param>
        public abstract void ExitTaskEvent(ChapterTask chapter);

        /// <summary>
        /// 一个封装好的函数，用于对于一些刚刚获取的物体，进行交互数据清空
        /// </summary>
        /// <param name="gameObject">需要进行清空的物体</param>
        public void DestoryObjAllInteracte(GameObject gameObject)
        {
            Interaction.InteractionInfo[] interactionInfos = gameObject.GetComponentsInParent<Interaction.InteractionInfo>();
            for(int i=0; i<interactionInfos.Length; i++)
            {
                GameObject.Destroy(interactionInfos[i]);
            }
        }

    }
}