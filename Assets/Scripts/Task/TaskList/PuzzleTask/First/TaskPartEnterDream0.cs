using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Task
{
    /// <summary>
    /// 入梦的第一个任务
    /// </summary>
    public class TaskPartEnterDream0 : TaskPart
    {
        public Vector3 oldManPos = new Vector3(653, 123, 765);
        /// <summary>        /// 第一个章节介绍是否显示完毕，在返回交互时会设置为true，表示第一章正式开始        /// </summary>
        GameObject oldMan;

        public TaskPartEnterDream0()
        {
            partName = "入梦"; 
            partDescribe = "一觉醒来，来到未知之地，心中有种追寻的冲动，但是我要找的是什么?";
        }
        public override void EnterTaskEvent(ChapterTask chapter)
        {
            Debug.Log("进入任务入梦");
            GameObject game = Resources.Load<GameObject>("NPC/OldMan");
            GameObject parent = GameObject.Find("NPC");
            oldMan = GameObject.Instantiate(game, parent.transform);
            Interaction.InteractionInfo[] taskInfos = oldMan.GetComponents<Interaction.InteractionInfo>();
            for(int i=0; i<taskInfos.Length; i++) GameObject.Destroy(taskInfos[0]);

            Interaction.TaskInfoCheck taskInfo = oldMan.AddComponent<Interaction.TaskInfoCheck>();
            taskInfo.SetChapterID(1);
            oldMan.transform.position = oldManPos;
            GameObject worldUi = TaskControl.Instance.worldUIPool.GetSimpleGameObject();
            worldUi.GetComponentInChildren<UnityEngine.UI.Text>().text = "梦之城混乱的日子又来了";
            worldUi.transform.parent = TaskControl.Instance.worldUIPool.transform;
            worldUi.transform.position = oldMan.transform.position + new Vector3(0,2,0);

        }

        public override void ExitTaskEvent(ChapterTask chapter)
        {
            GameObject.Destroy(oldMan);
            return;
        }

        public override bool IsCompleteTask(ChapterTask chapter, Interaction.InteractionInfo interactionInfo)
        {
            //需要注意一下该对话在一开始时就已经播放过了，所以这里直接放回退出任务
            //进入下一个剧情，播放下一个对话
            return true;
        }
    }
}