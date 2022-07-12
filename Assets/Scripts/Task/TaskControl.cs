
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FireControl.Task
{
    public class TaskControl : MonoBehaviour
    {
        private Common.ObjectPool.GameObjectPool objectPool;
        /// <summary>        /// 存储着已经获取的任务        /// </summary>
        private List<ChapterTask> tasks;
        private string allTaskPath = Application.streamingAssetsPath + "/Task/AllTask.task";
        private string completeTaskPath = Application.streamingAssetsPath + "/Task/CompleteTask.task";
        private string obtainTaskPath = Application.streamingAssetsPath + "/Task/ObtainTask.task";
        /// <summary>        /// 显示在摄像机中的UI的对外交互组件对象        /// </summary>
        private UI.UIExternalSimpleCommunicate uiTaskCommunicate;

        /// <summary>        /// 用来给外界检测该是否该启动该任务        /// </summary>
        public List<int> completeTasks = null;
        /// <summary>        /// 用来存储已经获取的章节的名称，用来加载任务时使用        /// </summary>
        private List<string> obtainTasks = null;

        static TaskControl taskControl;

        /// <summary>        /// 进行世界显示的UI的UI管理组件的UI名称        /// </summary>
        public string worldUIControlName = "Canvas_World";

        //[HideInInspector]
        /// <summary>        /// 用于在世界显示的特定UI的总控制组件        /// </summary>
        public UI.WorldUIObjPool worldUIPool;

        //由于出现有加载成功的情况，所以放到Awake中
        public void Awake()
        {
            GameObject temp = GameObject.Find(worldUIControlName);
            if (temp == null) return;
            worldUIPool = temp.GetComponent<UI.WorldUIObjPool>();
            if (taskControl == null)
                taskControl = this;
            if (worldUIPool == null) Debug.Log("世界UI生成的对象池为空");
        }

        private void Start()
        {
            objectPool = Common.ObjectPool.GameObjectPool.Instance;

            GameObject obj = GameObject.Find("Canvas_Screen");
            if (obj != null)
            {
                uiTaskCommunicate = obj.GetComponent<UI.UIExternalSimpleCommunicate>();
            }
            StartCoroutine(this.LoadTask());

        }

        public static TaskControl Instance
        {
            get
            {
                if(taskControl == null)
                {
                    GameObject task = new GameObject("Task");
                    taskControl = task.AddComponent<TaskControl>();
                }
                return taskControl;
            }
        }
        /// <summary>
        /// 清空对象，让该类变回初始状态
        /// </summary>
        public static void ReleaseInstance()
        {
            if(taskControl != null)
            {
                DestroyImmediate(taskControl.gameObject);
                taskControl = null;
            }
        }

        /// <summary>
        /// 添加章节函数，用于给添加任务类型的交互类型调用
        /// </summary>
        /// <param name="chapter">章节名称</param>
        /// <returns>是否添加成功</returns>
        public bool AddChapter(ChapterTask chapter)
        {
            if(tasks == null)
            {
                tasks = new List<ChapterTask> { chapter };
                chapter.BeginChapter();
                SaveObtainTask();
                return true;
            }
            else
            {
                for(int i=0; i<tasks.Count; i++)
                {
                    if(tasks[i].chapterID == chapter.chapterID)
                    {
                        Debug.Log("出现重复任务");
                        return false;
                    }
                }
                tasks = new List<ChapterTask> { chapter };
                chapter.BeginChapter();
                SaveObtainTask();
                return true;
            }
        }

        /// <summary>
        /// 加载任务系统
        /// </summary>
        public IEnumerator LoadTask()
        {
            //延后一帧，任务加载要等到初始化完成后再调用，保证所有准备都搞定了先
            yield return null;
            //加载获取了的任务
            LoadObtainTask();
            yield return null;
            //读取所有的任务数据
            string allTaskStr = Common.FileLoad.FileReadAndWrite.DirectReadFile(allTaskPath);
            string[] allTasks = null;
            if (allTaskStr != null && !allTaskStr.Equals(""))
            {
                allTasks = allTaskStr.Split('\n');
            }
            yield return null;

            string completeTask = Common.FileLoad.FileReadAndWrite.DirectReadFile(completeTaskPath);
            //赋值拥有的任务列表，只有有存在的任务时completeTasks才会有值，否则一直为null
            if (completeTask != null && !completeTask.Equals(""))
            {
                string[] comTasks = completeTask.Split('\n');
                if(comTasks != null && comTasks.Length > 0)
                {
                    completeTasks = new List<int>();
                    for(int i=0; i< comTasks.Length; i++)
                    {
                        int value;
                        if (int.TryParse(comTasks[i], out value))
                        {
                            completeTasks.Add(value);
                        }
                    }
                }
            }
            yield return null;

            //加载可以开启的任务
            if(allTasks != null)
            {
                string chapterPrefix = "FireControl.Task.ChapterTask";
                Assembly assembly = Assembly.GetExecutingAssembly();
                for (int i=0; i< allTasks.Length; i++)
                {
                    //如果有已经获取的任务时，需要加载判断该任务是否已经获取
                    if(obtainTasks != null)
                    {
                        bool isHave = false;
                        for(int j=0; j<obtainTasks.Count; j++)
                        {
                            if (allTasks[i].Equals(obtainTasks[j]))
                            {
                                isHave = true;
                                break;
                            }
                        }
                        //保证是未获取的任务才开始加载
                        if (!isHave)
                        {
                            ChapterTask chapterTask = (ChapterTask)assembly.CreateInstance(chapterPrefix + allTasks[i]);
                            chapterTask.CheckChapter();
                        }
                        yield return null;
                    }
                    else
                    {
                        ChapterTask chapterTask = (ChapterTask)assembly.CreateInstance(chapterPrefix + allTasks[i]);
                        chapterTask.CheckChapter();
                        yield return null;
                    }
                    
                }
            }
            //清空不需要的数据
            if(completeTasks != null)
            {
                completeTasks.Clear();
                completeTasks = null;
            }
            if(obtainTasks != null)
            {
                obtainTasks.Clear();
                obtainTasks = null;
            }
        }

        /// <summary>
        /// 加载获取了的任务的文件
        /// 任务存储格式：<ChapterName nowTaskIndex>，也就是第一个是任务名称，空格后保存当前任务的编号
        /// </summary>
        public void LoadObtainTask()
        {
            tasks = new List<ChapterTask>();
            List<string> task = Common.FileLoad.FileReadAndWrite.ReadFileByAngleBrackets(obtainTaskPath);
            if(task != null && task.Count > 0)
            {
                //Debug.Log("任务有值");
                string chapterPrefix = "FireControl.Task.ChapterTask";
                Assembly assembly = Assembly.GetExecutingAssembly();
                this.obtainTasks = new List<string>();
                for (int i=0; i< task.Count; i++)
                {
                    string[] tremps = task[i].Split(' ');
                    obtainTasks.Add(tremps[0]);
                    ChapterTask chapterTask = (ChapterTask)assembly.CreateInstance(chapterPrefix + tremps[0]);
                    //加入真正的章节存储数组，obtain只是检测用到，不是正在的存储数组
                    tasks.Add(chapterTask);
                    chapterTask.SetNowTaskPart(int.Parse(tremps[1]));
                }
                task.Clear();
                task = null;
            }
            else
            {
                //释放空间不要浪费
                if(task != null)
                    task.Clear();
                task = null;
            }
            
        }

        /// <summary>
        /// 任务完成的通用行为，将该任务退出，然后保存文件
        /// </summary>
        /// <param name="chapter">要完成的任务</param>
        public void CompleteChapter(ChapterTask chapter)
        {
            //调用退出函数
            StartCoroutine(chapter.ExitChapter());
            //写入文件
            string completeTask = Common.FileLoad.FileReadAndWrite.DirectReadFile(completeTaskPath);
            completeTasks = new List<int>();
            if (completeTask != null && !completeTask.Equals(""))
            {
                string[] comTasks = completeTask.Split('\n');
                if (comTasks != null && comTasks.Length > 0)
                {
                    for (int i = 0; i < completeTasks.Count; i++)
                        completeTasks.Add(int.Parse(comTasks[i]));
                }
            }
            completeTasks.Add(chapter.chapterID);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
            for(int i=0; i<completeTasks.Count; i++)
            {
                //完成的任务文件存储的是任务的ID
                stringBuilder.Append(completeTasks[i].ToString() + '\n');
            }
            Common.FileLoad.FileReadAndWrite.WriteFile(completeTaskPath, stringBuilder.ToString());
            stringBuilder.Clear();
            completeTasks.Clear();
            completeTasks = null;


            //移除拥有的任务
            for(int i=0; i<tasks.Count; i++)
            {
                if(tasks[i].chapterID == chapter.chapterID)
                {
                    tasks.RemoveAt(i);
                    SaveObtainTask();
                    return;
                }
            }
        }

        /// <summary>
        /// 保存当前存储的任务信息，用来进行获取的任务数据更新
        /// 当某个任务完成时会调用
        /// </summary>
        public void SaveObtainTask()
        {
            System.Text.StringBuilder obtainString = new System.Text.StringBuilder("");
            for(int i=0; i<tasks.Count; i++)
            {
                obtainString.Append("<" + tasks[i].chapterName + " " + tasks[i].nowCompletePart.ToString() + ">\n");
            }
            Common.FileLoad.FileReadAndWrite.WriteFile(obtainTaskPath, obtainString.ToString());
            obtainString.Clear();
        }

        public void CheckChapter(Interaction.InteracteTaskInfo taskInfo)
        {
            for (int i=0; i<tasks.Count; i++)
            {
                if(tasks[i].chapterID == taskInfo.ChapterID)
                {
                    tasks[i].CheckTask(taskInfo);
                    return;
                }
            }

            //如果调用了该函数，就是说明这里没有对应任务
            Interaction.InteractionControl.Instance.StopInteraction();
        }

        /// <summary>
        /// 读取该章节的选定文本,然后发送给UI进行内容显示
        /// </summary>
        /// <param name="chapterTask">章节名称</param>
        /// <param name="loadTextIndex">由于文本内容可能有许多不同，一节中也可以有多段对话，
        /// 所以使用一个特定的编号进行文本读取</param>
        /// <param name="onEndRun">在任务播完后执行的事情</param>
        public void LoadTaskText(ChapterTask chapterTask, int loadTextIndex, Common.Handler.HandlerList.INonReturnAndNonParam onEndRun)
        {
            string str = Common.FileLoad.FileReadAndWrite.ReadFileByAngleBrackets(
                chapterTask.chapterSavePath)[loadTextIndex];
            StartCoroutine(uiTaskCommunicate.BeginTask(str, onEndRun));
            return;
        }

        /// <summary>
        /// 通过编号获取任务章节
        /// </summary>
        public ChapterTask GetChapterByIndex(int index)
        {
            return tasks[index];
        }

        public List<ChapterTask> GetTask()
        {
            //Debug.Log("Task Size=" + tasks.Count);
            return tasks;
        }

        /// <summary>
        /// 返回开始状态，重置所有任务
        /// </summary>
        public void ReturnBegin()
        {
            Common.FileLoad.FileReadAndWrite.WriteFile(completeTaskPath, "");
            Common.FileLoad.FileReadAndWrite.WriteFile(obtainTaskPath, "");
        }
    }
}