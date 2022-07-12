
using UnityEngine;
using System.Collections.Generic;

namespace UI
{
    /// <summary>
    /// 所有UI动画显示时调用的方法，用来获取任务组件确定当前具有的UI，
    /// 然后进行生成，生成任务列表，这个是场景初始UI，一开始就放好的
    /// </summary>
    public class AllChapterUIControl : UIUseBase
    {
        private RectTransform m_RectTransform;
        private FireControl.Task.TaskControl m_TaskControl;
        /// <summary>        /// 显示任务的背景图片名称        /// </summary>
        public string chapterShowBackground = "Image_TaskBackground";
        /// <summary>        /// 用于显示章节任务内容的组件名称        /// </summary>
        public string chapterContentShow = "Panel_TaskContent";
        /// <summary>        /// 用来执行下面一切操作的根据UI，不直接获取是因为要一开始隐藏该UI        /// </summary>
        public GameObject setUIObj;
        public bool isShow;

        /// <summary>        /// 初始时根据当前屏幕像素加载UI大小        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if(setUIObj != null)
                m_RectTransform = setUIObj.GetComponent<RectTransform>();
            isShow = false;
        }

        private void Start()
        {
            m_TaskControl = FireControl.Task.TaskControl.Instance;
        }

        ///// <summary>
        ///// 加载全部任务,同时设置显示时需要的一切UI的显示
        ///// </summary>
        public void LoadAllChapter()
        {
            Debug.Log("Load Chapter");
            //判断能否显示
            List<FireControl.Task.ChapterTask> chapterTasks = m_TaskControl.GetTask();
            if (setUIObj == null) 
            {
                Debug.LogError("setUIObj is null");
            }
            //显示背景
            GameObject obj = control.UIObjectDictionary[chapterShowBackground];
            if (obj != null) obj.SetActive(true);
            else Debug.Log("obj is null");

            //设置描述内容
            GameObject taskContent = UICommon.Initialize.GetGameObject("Panel_TaskContent", control);
            taskContent.SetActive(true);
            //TaskContentShow taskContentShow = taskContent.GetComponent<TaskContentShow>();
            //ChapterTask chapterTask = chapterTasks[0];
            //if (taskContentShow == null) Debug.Log("TaskContentShow is null");
            //taskContentShow.ReSetContent(chapterTask.chapterName, chapterTask.GetPartName(), chapterTask.GetPartDescribe());

            //设置子物体
            int count = chapterTasks.Count;
            m_RectTransform.sizeDelta = new Vector2(0, -Camera.main.pixelHeight + 100 * count);
            GameObject taskEle = Resources.Load<GameObject>("UI/Panel_TaskElement");
            for(int i=0; i<count; i++)
            {
                GameObject temp = GameObject.Instantiate(taskEle, setUIObj.transform);
                temp.name = chapterTasks[i].chapterName;
                ChapterContentManege chapterContent = temp.GetComponentInChildren<ChapterContentManege>();
                chapterContent.taskIndex = i;
                temp.SetActive(true);
            }
            isShow = true;
            gameObject.SetActive(true);
            FireControl.Interaction.InteractionControl.Instance.StartInteraction();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FireControl.Interaction.InteractionControl.Instance.StopInteraction();
                gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// 清空数据，以及相关数据
        /// </summary>
        private void OnDisable()
        {
            //隐藏背景
            GameObject obj = control.UIObjectDictionary[chapterShowBackground];
            if (obj != null) obj.SetActive(false);

            //隐藏描述组件
            GameObject taskContent = UICommon.Initialize.GetGameObject("Panel_TaskContent", control);
            if(taskContent != null)
                taskContent.SetActive(false);

            int count = setUIObj.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(setUIObj.transform.GetChild(0).gameObject);
            }
        }
    }
}