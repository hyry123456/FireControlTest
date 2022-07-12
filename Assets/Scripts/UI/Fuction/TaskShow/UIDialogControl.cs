
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// UI的任务显示管理组件，挂在任务显示的UI父级上，用来管理之类的UI显示以及隐藏
    /// </summary>
    public class UIDialogControl : UIUseBase
    {
        /// <summary>        /// 说话的人的显示UI对象的名称        /// </summary>
        public string speakerName = "Text_SpeakerName";
        /// <summary>        /// 说话人的内容显示UI对象名称        /// </summary>
        public string speakerDescription = "Test_SpeakerDescription";
        /// <summary>        /// 任务显示背景名称        /// </summary>
        public string background = "Image_TaskBeground";

        protected Text speakerNameUI;
        protected Text speakerDescriptionUI;
        protected GameObject backgroundObj;
        protected List<string> descriptions;
        protected int nowIndex;
        Common.Handler.HandlerList.INonReturnAndNonParam onEndFunction;

        protected override void Awake()
        {
            base.Awake();
            control.init += ReadyTaskComponent;
            //显示自身，但是关闭子组件
        }

        /// <summary>
        /// 获取组件子组件,同时隐藏两个子组件
        /// </summary>
        protected void ReadyTaskComponent()
        {
            speakerNameUI = transform.Find(speakerName).GetComponent<Text>();
            speakerDescriptionUI = transform.Find(speakerDescription).GetComponent<Text>();
            backgroundObj = transform.Find(background).gameObject;
        }

        /// <summary>
        /// 时时检测按键输入的数据，进行对话的下一个阶段调用
        /// </summary>
        protected virtual void Update()
        {
            if (descriptions == null) return;
            //按下后加载新文本
            if (Input.GetMouseButtonDown(0))
            {
                nowIndex++;
                SetNowDialog();
            }
        }

        /// <summary>
        /// 开启对话
        /// </summary>
        /// <param name="allString">输入的所有对话内容</param>
        public void BeginDialog(string allString, Common.Handler.HandlerList.INonReturnAndNonParam onEndRun)
        {
            onEndFunction = onEndRun;
            FireControl.Interaction.InteractionControl.Instance.StartInteraction();
            descriptions = new List<string>();
            string[] strs = allString.Split('\n');
            descriptions.AddRange(strs);
            nowIndex = 0;
            gameObject.SetActive(true);
            //设置当前内容
            SetNowDialog();
        }

        /// <summary>
        /// 显示设置当前对话内容
        /// </summary>
        protected void SetNowDialog()
        {
            for(int i=nowIndex; i<descriptions.Count; i++)
            {
                string[] strs = descriptions[i].Split('$');
                if (strs.Length != 2)
                    continue;
                else
                {
                    //设置数值
                    speakerNameUI.text = strs[0];
                    speakerDescriptionUI.text = strs[1];
                    nowIndex = i;
                    return;
                }
            }
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (descriptions != null)
            {
                //结束后执行的行为
                if (onEndFunction != null) { 
                    onEndFunction(); 
                    onEndFunction = null;
                }
                descriptions.Clear();
                descriptions = null;
            }
        }


    }
}