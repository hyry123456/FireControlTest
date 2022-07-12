
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
        /// <summary>        /// ����������ߵ���ʼ��        /// </summary>
        public Transform beginPos;
        /// <summary>        /// ��ǰ���Դ����Ľ�����Ϣ        /// </summary>
        public InteractionInfo nowInteractionInfo;
        /// <summary>        /// �Ƿ����ڽ�����        /// </summary>
        public bool isInteracting;
        /// <summary>        /// ���߼��ľ���        /// </summary>
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

        /// <summary>        /// ������ǵ���Ϣ        /// </summary>
        public Info.PlayerInfo PlayerInfo
        {
            get { 
                return instance.PlayerInfo; 
            }
        }

        private InteractionControl() { }
        /// <summary>
        /// �ҵ������ϣ�������ʱʱ����
        /// </summary>
        private void Start()
        {
            instance = this;
            interacteCommunicate = UI.UIExternalSimpleCommunicate.GetUIExternalByRenderMode(RenderMode.ScreenSpaceOverlay);
            playerControl = gameObject.GetComponent<Control.PlayerControl>();
            isInteracting = false;
        }

        /// <summary>
        /// ����Ƿ��п��Խ����Ķ���
        /// </summary>
        protected void FixedUpdate()
        {
            //�����о��˳�
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
        /// ���н����¼�
        /// </summary>
        /// <param name="interactionInfo">�����Ľ����¼�</param>
        public void RunInteraction(InteractionInfo interactionInfo)
        {
            isInteracting = true;   //�������ڽ���
            interacteCommunicate.CloseInteracteRemind();
            if (interactionInfo == null) { 
                Debug.Log("�����������");
                //isInteracting = false;
                //return;
            }
            StartCoroutine(interactionInfo.InteractionBehavior(this));
        }

        /// <summary>
        /// ������ϵͳ��UI��ʾ��������ã���ʾ������UI�����ˣ�׼���������Ĳ�����
        /// </summary>
        public void ReRunInteraction()
        {
            RunInteraction(nowInteractionInfo);
        }

        /// <summary>
        /// �����˽�����ʱ���иú���
        /// </summary>
        public void PressInteraction()
        {
            if (nowInteractionInfo == null) return;
            RunInteraction(nowInteractionInfo);
        }

        /// <summary>
        /// ��ʾֹͣ��������ϵͳ���¿�ʼ����
        /// </summary>
        public void StopInteraction()
        {
            isInteracting = false;
        }

        /// <summary>
        /// ��������
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
        /// ��ӽ�����Ϣ�����ڵ������¼��������ߵ��ʱ����ʱ��Ҫ��ӽ�������Ҫ�ֶ������
        /// </summary>
        /// <param name="interaction">��ӵĽ�����Ϣ</param>
        public void AddInteractionInfo(InteractionInfo interaction)
        {
            nowInteractionInfo = interaction;
        }
    }
}