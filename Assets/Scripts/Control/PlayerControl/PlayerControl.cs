
using UnityEngine;
using Common.ResetInput;

namespace FireControl.Control
{
    //[RequireComponent(typeof(Skill.SkillManage))]
    //[RequireComponent(typeof(Interaction.InteractionControl))]
    //[RequireComponent(typeof(Equip.Bag))]
    public class PlayerControl : MonoBehaviour
    {
        private static PlayerControl instance;
        //protected Motor.FirstPersonMotor motor;
        //private Motor.FirstPersonMotor_World motor;
        private Motor.RigibodyMotor motor;

        //public GameObject view;
        //private Motor.ThirdPersonPective thirdPersonPective;
        public GameObject view;
        private Motor.OribitCamera oribitCamera;
        
        private bool locked;
        public bool IsLock
        {
            get
            {
                return locked;
            }
        }

        private bool isEagle;
        public bool IsEagle
        {
            get { return isEagle; }
        }



        protected Skill.SkillManage skillManage;

        /// <summary>        /// UI对外接口对象        /// </summary>
        private UI.UIExternalSimpleCommunicate uiInterface;

        private Interaction.InteractionControl interactionControl;
        private Equip.Bag equip;

        private Info.PlayerInfo playerInfo;
        public Info.PlayerInfo PlayerInfo
        {
            get {
                if (playerInfo == null)
                    playerInfo = GetComponent<Info.PlayerInfo>();
                return playerInfo;
            }
        }

        public static PlayerControl Instance {
            get
            {
                if(instance == null)
                    instance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
                return instance; 
            }
        }

        public string verticalName = "Vertical";
        public string horizontalName = "Horizontal";
        public string jumpName = "Jump";
        public string setName = "Setting";
        public string begName = "Bag";
        public string interacteName = "Interaction";
        public string rollName = "Roll";
        public string runName = "Run";

        private void Awake()
        {
            instance = this;
        }

        protected void Start()
        {
            //motor = GetComponent<Motor.FirstPersonMotor>();
            motor = GetComponent<Motor.RigibodyMotor>();
            interactionControl = GetComponent<Interaction.InteractionControl>();
            equip = GetComponent<Equip.Bag>();
            uiInterface = UI.UIExternalSimpleCommunicate.GetUIExternalByRenderMode(RenderMode.ScreenSpaceOverlay);
            skillManage = GetComponent<Skill.SkillManage>();
            playerInfo = GetComponent<Info.PlayerInfo>();

            //if (view == null) view = GameObject.Find("View");
            oribitCamera = view.GetComponent<Motor.OribitCamera>();
            locked = false;
        }

        /// <summary>
        /// 时时刷新的控制属性的存放位置
        /// </summary>
        private void Update()
        {
            if(interactionControl.GetInteraction())return;

            float xMouse = Input.GetAxis("Mouse X");
            float yMouse = -Input.GetAxis("Mouse Y");
            
            if (PlayerInfo.IsHardStraight) return;

            if (uiInterface != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("显示UI");
                    StartCoroutine(uiInterface.ShowAllTask());
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isEagle = true;
            }

            if (Input.GetKeyUp(KeyCode.Tab))
                isEagle = false;
            //motor.Rotate(xMouse, yMouse, PlayerInfo.rotateSpeed);
            oribitCamera.SetCameraInput(yMouse, xMouse);
            if (Input.GetMouseButton(0))
                equip.RunWeapon();
        }
        /// <summary>
        /// 物理帧刷新的属性计算位置，一些没有必要逐帧计算的可以在这里进行计算
        /// </summary>
        private void FixedUpdate()
        {
            //只有非交互状态时才可以控制
            if (interactionControl.GetInteraction() || PlayerInfo.IsHardStraight) return;
            //先获取这些，之后补充其他
            float vertical = MyInput.Instance.GetAsis(verticalName);
            float horizontal = MyInput.Instance.GetAsis(horizontalName);
            bool intreracte = MyInput.Instance.GetButtonDown(interacteName);
            bool bag = MyInput.Instance.GetButtonDown(begName);
            bool run = MyInput.Instance.GetButton(runName);
            bool jump = MyInput.Instance.GetButtonDown(jumpName);

            if (bag)
                StartCoroutine(uiInterface.ShowSetting());

            if (intreracte)
                interactionControl.PressInteraction();

            //if (MyInput.Instance.GetButtonDown(rollName))
            //{

            //    List<Skill.SkillBase> list = skillManage.GetCanUseSkillByType(Skill.SkillType.Dodge);
            //    skillManage.CheckAndRelase(Common.CommonFunction.ChoseOneOnList(list));
            //    if(list != null)
            //    {
            //        list.Clear();
            //        list = null;
            //    }
            //}


            motor.Move(horizontal, vertical);
            if (jump)
                motor.DesireJump();
        }

        /// <summary>
        /// 获得主角看向的位置，也就是摄像机前方
        /// </summary>
        public Vector3 GetLookatDir()
        {
            if (Camera.main == null) return Vector3.zero;
            return Camera.main.transform.forward;
        }

        /// <summary>
        /// 获得摄像机的世界坐标
        /// </summary>
        public Vector3 GetCameraPos()
        {
            Camera camera = Camera.main;
            if (camera != null)
                return camera.transform.position;
            else return Vector3.zero;
        }

        ///// <summary>
        ///// 获得移动基类
        ///// </summary>
        //public Motor.Motor GetMotor()
        //{
        //    return motor;
        //}
    }
}