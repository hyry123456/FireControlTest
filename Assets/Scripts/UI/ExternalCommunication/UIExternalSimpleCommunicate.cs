
using System.Collections;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// UI外界交互任务系列，添加了任务行为，对于有需要任务交流的画布就挂上这个组件
    /// 不要挂父类，否者不能正确调用任务显示
    /// </summary>
    public class UIExternalSimpleCommunicate : UIExternalCommunicate
    {
        /// <summary>        /// 该场景的静态主要UI对外接口对象        /// </summary>
        private static UIExternalSimpleCommunicate screenOverLay;
        private static UIExternalSimpleCommunicate worldSpace;

        /// <summary>
        /// 通过UI渲染类型获取UI对外对象，因为通常同一种类型的UI会放在同一个Canvas下
        /// 一般只需要screenOverLay还有World就够了，一个静态显示，一个世界显示
        /// </summary>
        /// <param name="renderMode">渲染类型</param>
        /// <returns>对应的接口对象</returns>
        public static UIExternalSimpleCommunicate GetUIExternalByRenderMode(RenderMode renderMode)
        {
            switch (renderMode)
            {
                case RenderMode.ScreenSpaceOverlay: return screenOverLay;
                case RenderMode.WorldSpace: return worldSpace;
                default: return null;
            }
        }



        /// <summary>        /// UI的对话的管理逐渐挂载位置名        /// </summary>
        public string uiDialogControlName = "Panel_Dialog";
        /// <summary>        /// 任务列表的UI管理组件名称        /// </summary>
        public string chapterUiControl = "Scroll View_ChapterList";
        /// <summary>        /// 交互的提示UI的管理类挂载的位置名称，一般用来设置名称而已     /// </summary>
        public string interacteReminderControlName = "Panel_InteracteReminder";
        /// <summary>        /// 装备信息显示的UI管理逐渐名称        /// </summary>
        public string equipInfoShowControlName = "Panel_ShowEquipInfo";
        public string settingControlName = "Panel_Setting";

        protected UIDialogControl taskControl;
        protected AllChapterUIControl allChapterUIControl;
        protected InteracteRemindUI interacteRemindUI;
        protected EquipUIControl equipUIControl;
        protected Setting.SettingControl settingControl;
        /// <summary>        /// 命令队列，当需要停止UI命令，同时又有UI命令进行执行时调用        /// </summary>
        protected Queue commonQueue;
        public bool isUnCommunicate = false;

        /// <summary>        /// 初始化对外接口对象        /// </summary>
        protected void Awake()
        {
            Canvas canvas = GetComponent<Canvas>();
            switch (canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay: {
                        if (screenOverLay != null) break;
                        screenOverLay = this; break;
                    }
                case RenderMode.WorldSpace: {
                        if (worldSpace != null) break;
                        worldSpace = this; break; }
            }
        }

        protected override void Start()
        {
            base.Start();
            control.init += TaskInit;
        }


        /// <summary>
        /// 任务初始化行为，用来加载UI的任务组件
        /// </summary>
        public void TaskInit()
        {
            GameObject game;
            if(control.UIObjectDictionary.TryGetValue(uiDialogControlName, out game))
            {
                taskControl = game.GetComponent<UIDialogControl>();
            }
            if (control.UIObjectDictionary.TryGetValue(chapterUiControl, out game))
            {
                allChapterUIControl = game.GetComponent<AllChapterUIControl>();
            }
            if (control.UIObjectDictionary.TryGetValue(interacteReminderControlName, out game))
            {
                interacteRemindUI = game.GetComponent<InteracteRemindUI>();
            }
            if (control.UIObjectDictionary.TryGetValue(equipInfoShowControlName, out game))
            {
                equipUIControl = game.GetComponent<EquipUIControl>();
            }
            if(control.UIObjectDictionary.TryGetValue(settingControlName, out game))
            {
                settingControl = game.GetComponent <Setting.SettingControl>();
            }
        }

        /// <summary>
        /// 开启任务对话UI显示
        /// </summary>
        /// <param name="taskString">对话文本</param>
        public IEnumerator BeginTask(string taskString, Common.Handler.HandlerList.INonReturnAndNonParam onEndRun)
        {
            while (isUnCommunicate)
                yield return null;
            if (taskControl == null)
            {
                Debug.Log("UI任务组件为空");
                yield break;
            }
            taskControl.BeginDialog(taskString, onEndRun);
        }


        /// <summary>
        /// 该方法需要协程调用，防止一当显示就退出的可能
        /// </summary>
        public IEnumerator ShowAllTask()
        {
            while (isUnCommunicate)
            {
                Debug.Log("communicate");
                yield return null;
            }
            allChapterUIControl.LoadAllChapter();
        }

        /// <summary>
        /// 显示交互UI，根据交互不同显示不同的值
        /// </summary>
        /// <param name="interactionType">交互类型</param>
        public void StartInteracteRemind(FireControl.Interaction.InteractionType interactionType)
        {
            if (isUnCommunicate) return;
            interacteRemindUI.SetAndShowReminderText(interactionType);
        }

        /// <summary>
        /// 关闭交互提醒UI
        /// </summary>
        public void CloseInteracteRemind()
        {
            //这个为空不一定错，可能是初始化的先后顺序不同
            if (interacteRemindUI == null)
                return;
            interacteRemindUI.gameObject.SetActive(false);
        }

        /// <summary>     /// 使用UI显示获取到的物品信息     /// </summary>
        public IEnumerator ShowEquipInfo(string infoContent, string equipName)
        {
            while (isUnCommunicate)
            {
                yield return null;
            }
            equipUIControl.ShowEquipInfo(infoContent, equipName);
        }

        /// <summary>        /// 显示设置界面        /// </summary>
        public IEnumerator ShowSetting()
        {
            while (isUnCommunicate)
            {
                yield return null;
            }
            settingControl.ShowSetting();
        }
        /// <summary>        /// 关闭所有的显示的UI，同时设置所有消息为不可显示        /// </summary>
        public void CloseAllPlane()
        {
            isUnCommunicate = true;
            if (taskControl != null)
                taskControl.gameObject.SetActive(false);
            if(allChapterUIControl != null)
                allChapterUIControl.gameObject.SetActive(false);
            if(interacteRemindUI != null)
                interacteRemindUI.gameObject.SetActive(false);
            //equipUIControl.gameObject.SetActive(false);
            if(settingControl != null)
                settingControl.gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置所有UI可以开始显示
        /// </summary>
        public void CanShowPlane()
        {
            isUnCommunicate = false;
        }

    }
}