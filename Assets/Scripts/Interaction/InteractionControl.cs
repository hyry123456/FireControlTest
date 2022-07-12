
using UnityEngine;

namespace FireControl.Interaction
{
    public class InteractionControl : MonoBehaviour
    {
        //public string interacteHintUI = "Text_InteractionPromote";
        //public string mainCanvesName = "Canvas_Screen";

        private UI.UIExternalSimpleCommunicate interacteCommunicate;
        private static InteractionControl instance;
        private Control.PlayerControl playerControl;
        /// <summary>        /// 交互检测射线的起始点        /// </summary>
        public Transform beginPos;
        /// <summary>        /// 当前可以触发的交互信息        /// </summary>
        public InteractionInfo nowInteractionInfo;
        /// <summary>        /// 是否正在交互中        /// </summary>
        public bool isInteracting;
        /// <summary>        /// 射线检测的距离        /// </summary>
        public float interacteCheckDistance = 3f;
        public static InteractionControl Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("intreraction");
                    instance = go.AddComponent<InteractionControl>();
                    go.hideFlags = HideFlags.HideAndDontSave;
                }
                return instance;
            }
        }

        /// <summary>        /// 获得主角的信息        /// </summary>
        public Info.PlayerInfo PlayerInfo
        {
            get { 
                return instance.PlayerInfo; 
            }
        }

        private InteractionControl() { }
        /// <summary>
        /// 挂到主角上，由主角时时交互
        /// </summary>
        private void Start()
        {
            instance = this;
            interacteCommunicate = UI.UIExternalSimpleCommunicate.GetUIExternalByRenderMode(RenderMode.ScreenSpaceOverlay);
            playerControl = gameObject.GetComponent<Control.PlayerControl>();
            isInteracting = false;
        }

        /// <summary>
        /// 检查是否有可以交互的对象
        /// </summary>
        protected void FixedUpdate()
        {
            //交互中就退出
            if (isInteracting || interacteCommunicate == null) 
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(playerControl.GetCameraPos(), playerControl.GetLookatDir(), out hit, interacteCheckDistance))
            {
                InteractionInfo hitInfo = hit.transform.GetComponent<InteractionInfo>();
                if (hitInfo != null && (hitInfo.interactionType & InteractionType.Enemy) == 0)
                {
                    nowInteractionInfo = hitInfo;
                    interacteCommunicate.StartInteracteRemind(hitInfo.interactionType);
                    return;
                }
                nowInteractionInfo = null;
            }
            this.interacteCommunicate.CloseInteracteRemind();
        }

        /// <summary>
        /// 运行交互事件
        /// </summary>
        /// <param name="interactionInfo">发生的交互事件</param>
        public void RunInteraction(InteractionInfo interactionInfo)
        {
            isInteracting = true;   //设置正在交互
            interacteCommunicate.CloseInteracteRemind();
            if (interactionInfo == null) { 
                Debug.Log("交互对象空了");
                //isInteracting = false;
                //return;
            }
            StartCoroutine(interactionInfo.InteractionBehavior(this));
        }

        /// <summary>
        /// 有任务系统的UI显示结束后调用，表示交互的UI结束了，准备接下来的操作吧
        /// </summary>
        public void ReRunInteraction()
        {
            RunInteraction(nowInteractionInfo);
        }

        /// <summary>
        /// 按下了交互键时运行该函数
        /// </summary>
        public void PressInteraction()
        {
            if (nowInteractionInfo == null) return;
            RunInteraction(nowInteractionInfo);
        }

        /// <summary>
        /// 表示停止交互，该系统重新开始工作
        /// </summary>
        public void StopInteraction()
        {
            isInteracting = false;
        }

        /// <summary>
        /// 开启交互
        /// </summary>
        public void StartInteraction()
        {
            isInteracting = true;
        }

        public bool GetInteraction()
        {
            return isInteracting;
        }

        /// <summary>
        /// 添加交互信息，由于当交互事件不是射线点击时触发时需要添加交互就需要手动添加了
        /// </summary>
        /// <param name="interaction">添加的交互信息</param>
        public void AddInteractionInfo(InteractionInfo interaction)
        {
            nowInteractionInfo = interaction;
        }
    }
}