using FireControl.Interaction;
using UnityEngine;

namespace FireControl.Task
{
    public class TaskPartEnterDream1 : TaskPart
    {
        private Vector3 oldManPos = new Vector3(653, 123, 765);
        private Vector3 bookPos = new Vector3(662, 123, 740);
        GameObject oldMan;
        public TaskPartEnterDream1()
        {
            partName = "探索周围";
            partDescribe = "没有尽头的走廊，无尽又重复的房间，真不愧是梦吗";
        }

        public override void EnterTaskEvent(ChapterTask chapter)
        {
            Debug.Log("加载入梦第二章 " + chapter.chapterID);

            Interaction.InteractionControl.Instance.StartInteraction();
            //添加一个交互行为,用来当任务播放完成时进行停止交互，毕竟这个
            //任务播放不是接触触发的

            TaskControl.Instance.LoadTaskText(chapter, 1, () =>
            {
                Interaction.InteractionControl.Instance.StopInteraction();
            });
            GameObject game = Resources.Load<GameObject>("NPC/OldMan");
            GameObject parent = GameObject.Find("NPC");
            oldMan = GameObject.Instantiate(game, parent.transform);
            Interaction.InteractionInfo[] taskInfos = oldMan.GetComponents<Interaction. InteractionInfo>();
            for (int i = 0; i < taskInfos.Length; i++) GameObject.Destroy(taskInfos[0]);

            //设置老人位置
            oldMan.transform.position = oldManPos;
            GameObject worldUi = TaskControl.Instance.worldUIPool.GetSimpleGameObject();
            worldUi.GetComponentInChildren<UnityEngine.UI.Text>().text = "梦之边缘，迷失之地，无尽之梦，何为真又何为假啊";
            worldUi.transform.parent = TaskControl.Instance.worldUIPool.transform;
            worldUi.transform.position = oldMan.transform.position + new Vector3(0, 2, 0);

            GameObject book = GameObject.Instantiate(Resources.Load<GameObject>("Item/Book"), GameObject.Find("Item").transform);
            book.SetActive(true);
            Debug.Log(book.name);
            book.transform.position = bookPos;
            //查找以及删除现有交互行为
            Interaction.InteractionInfo[] taskInfos1 = oldMan.GetComponents<Interaction.InteractionInfo>();
            for (int i = 0; i < taskInfos1.Length; i++) GameObject.Destroy(taskInfos1[0]);

            FireControl.Interaction.TaskInfoCheck bookInteracte = book.AddComponent<Interaction.TaskInfoCheck>();
            bookInteracte.SetChapterID(chapter.chapterID);
            //做一个简单的区分，默认是0
            bookInteracte.interactionID = 1;
            bookInteracte.nonReAndNonParam = () =>
            {
                Control.PlayerControl.Instance.gameObject.GetComponent<Equip.Bag>().AddEquip(new Equip.Item_Book());
                GameObject.Destroy(book);
            };
            
        }

        public override void ExitTaskEvent(ChapterTask chapter)
        {
            return;
        }

        public override bool IsCompleteTask(ChapterTask chapter, Interaction.InteractionInfo interactionInfo)
        {
            if(interactionInfo.interactionID == 1)
            {
                //添加回调事件，开启交互
                TaskControl.Instance.LoadTaskText(chapter, 2, () =>
                {
                    Interaction.InteractionControl.Instance.StopInteraction();
                });

                Interaction.InteractionControl.Instance.StartInteraction();

                Interaction.InteractionInfo[] taskInfos1 = oldMan.GetComponents<Interaction.InteractionInfo>();
                for (int i = 0; i < taskInfos1.Length; i++) GameObject.Destroy(taskInfos1[0]);
                
                FireControl.Interaction.TaskInfoCheck taskInfoCheck = oldMan.AddComponent<FireControl.Interaction.TaskInfoCheck>();
                taskInfoCheck.SetChapterID(chapter.chapterID);
                taskInfoCheck.interactionID = 2;

                return false;
            }
            if(interactionInfo.interactionID == 2)
            {
                TaskControl.Instance.LoadTaskText(chapter, 3, () =>
                {
                    Debug.Log("先黑屏");
                });
                Interaction.InteractionControl.Instance.StartInteraction();
                return true;
            }

            return false; 
        }

    }
}