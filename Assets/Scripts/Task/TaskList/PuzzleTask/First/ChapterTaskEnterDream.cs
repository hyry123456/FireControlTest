using FireControl.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Task
{

    public class ChapterTaskEnterDream : ChapterTaskSimple
    {
        /// <summary>
        /// 迷宫场景的第一个任务
        /// </summary>
        public ChapterTaskEnterDream()
        {
            chapterName = "EnterDream";
            chapterTitle = "有求与梦，方能入梦";
            taskPartSize = 2;
            chapterID = 1;
            //任务文件用编号命名
            chapterSavePath = Application.streamingAssetsPath + "/" + "Task/Chapter/1.task";
            targetPart = targetPart + chapterName;
        }

        public override void CheckChapter()
        {
            List<int> vs = TaskControl.Instance.completeTasks;
            //没有完成的任务所以是开始阶段
            if (vs == null)
            {
                LoadChapter();
            }
            else
            {
                //只要没有完成该任务就加载
                if (!vs.Contains(1))
                {
                    LoadChapter();
                }
            }

        }

        protected override void LoadChapter()
        {
            InteractionControl.Instance.StartInteraction();
            //保证加载的任务是第一个文本
            nowCompletePart = 0;
            //添加交互事件,也就是章节添加事件，需要注意的是在对话时任务还没有添加成功
            //不用担心如果任务中途退出时会导致的Bug
            TaskInfoAddChapter taskInfo = new TaskInfoAddChapter();
            taskInfo.addTaskName = "FireControl.Task.ChapterTaskEnterDream";
            //添加交互停止行为
            //taskInfo.nonReAndNonParam = () =>
            //{
            //    InteractionControl.Instance.StopInteraction();
            //};
            
            //InteractionControl.Instance.AddInteractionInfo(taskInfo);
            //读取第一个文本，需要注意的是我这里其实并没有正在的将章节加入到列表中
            //仅仅只是播放了第一章的内容而已
            TaskControl.Instance.LoadTaskText(this, 0, () =>
            {
                InteractionControl.Instance.StopInteraction();
            });
        }

        public override IEnumerator ExitChapter()
        {
            yield break;
        }

    }
}