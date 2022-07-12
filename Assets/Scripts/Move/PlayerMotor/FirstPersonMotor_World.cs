
using UnityEngine;


namespace FireControl.Motor
{
    public class FirstPersonMotor_World : MonoBehaviour
    {

        /// <summary>        /// 存储上一个位置        /// </summary>
        private Vector3 prePos;
        /// <summary>        /// 上一次移动值        /// </summary>
        private Vector3 preMove;
        /// <summary>        /// 角色控制器        /// </summary>
        private CharacterController character;
        /// <summary>       /// 是否滑动中        /// </summary>
        public bool isSlip;
        /// <summary>        /// 滑动的实际方向        /// </summary>
        private Vector3 slipTrue;
        /// <summary>        /// 角色信息控制器        /// </summary>
        private Info.PlayerInfo playerInfo;

        public float waitTime = 0f;

        /// <summary>        /// 是否正在暂停中        /// </summary>
        protected bool IsWating
        {
            get
            {
                if(waitTime <= 0)
                    return false;
                return true;
            }
        }

        /// <summary>        /// 是否触地        /// </summary>
        public bool isGround;
        /// <summary>        /// 是否触墙        /// /// </summary>
        public bool isWall;
        [Range(0, 20f)]
        /// <summary>        /// 重力        /// </summary>
        public float gravity = 9.8f;
        /// <summary>        /// 是否需要设置摄像机位置        /// </summary>
        public bool needSetCameraPos = true;
        /// <summary>        /// 移动速度        /// </summary>
        public Vector3 moveSpeed;
        [Range(0, 1)]
        /// <summary>        /// 摩檫力        /// </summary>
        public float friction = 0.05f;
        /// <summary>        /// 移动角度，当大于该角度时会开始站不稳，然后滑动        /// </summary>
        public float moveAngle = 60;
        /// <summary>        /// 最大的滑动速度        /// </summary>
        public float maxSlipSpeed = 10;
        /// <summary>        /// 模型检测用的遮罩        /// </summary>
        public LayerMask checkMask;
        /// <summary>        /// 攀爬检测线的外扩距离        /// </summary>
        public float forwardClimbCheck = 0.3f;
        /// <summary>        /// 攀爬检测线向上值        /// </summary>
        public float upClimbCheck = 1f;

        /// <summary>        /// 当暂停结束后执行的方法        /// </summary>
        protected Common.Handler.HandlerList.INonReturnAndNonParam onWaitEnd;

        protected Vector3 CharacterPos
        {
            get
            {
                if (character == null)
                    return Vector3.zero;
                return character.transform.position;
            }
        }

        protected virtual void Start()
        {
            moveSpeed = Vector3.zero;
            prePos = moveSpeed;
            character = GetComponent<CharacterController>();
            if (Camera.main != null)
                Camera.main.transform.localRotation = Quaternion.identity;
            playerInfo = GetComponentInChildren<Info.PlayerInfo>();
        }

        protected virtual void FixedUpdate()
        {
            UpdateWaitTime();
            if (IsWating)
                return;

            if (character == null) return;
            Vector3 trueMove = prePos - CharacterPos;
            Matrix4x4 matrix4X4 = Matrix4x4.TRS(CharacterPos, character.transform.rotation, character.transform.localScale);

            //当在斜面时，返回false，不需要执行后面的代码
            if (!CheckGround(trueMove, matrix4X4))
                return;

            SetAnimateValue();

            //设置摄像机还有角色的旋转
            SetCamera();

            WallCheckAndDecreaseMove(trueMove, matrix4X4);
        }

        /// <summary>        /// 更新等却时间        /// </summary>
        private void UpdateWaitTime()
        {
            if (waitTime > 0)
            {
                waitTime -= Time.fixedDeltaTime;
                if(waitTime < 0)
                    if(onWaitEnd != null)
                    {
                        onWaitEnd();
                        onWaitEnd = null;
                    }
            }
        }

        /// <summary>
        /// 地面以及斜面检测还有攀爬检测
        /// </summary>
        /// <param name="trueMove">实际移动值</param>
        /// <param name="matrix4X4">对应矩阵</param>
        /// <returns>返回是否需要继续执行</returns>
        private bool CheckGround(Vector3 trueMove, Matrix4x4 matrix4X4)
        {
            //避免在跳跃时出现清除
            if (moveSpeed.y < 0)
            {
                //触底，可能撞到上方会出现bug
                if (Mathf.Abs(trueMove.y) < Mathf.Abs(preMove.y) * 0.9f)
                {
                    //检查斜面
                    RaycastHit hit;
                    if (Physics.Raycast(CharacterPos, -character.transform.up, out hit, character.height, checkMask))
                    {

                        float angle = Vector3.Angle(hit.normal.normalized, character.transform.up);
                        //是地面
                        if (angle < moveAngle)
                        {
                            isGround = true;
                            moveSpeed.y = 0;
                            isSlip = false;
                        }
                        else
                        {
                            //新开始的滑动
                            if (isSlip == false)
                            {
                                float cos = Vector3.Dot(hit.normal.normalized, character.transform.up);
                                float sin = Mathf.Sqrt(1 - cos * cos);
                                Matrix4x4 rotateZMat = new Matrix4x4(new Vector4(sin, -cos, 0, 0),
                                    new Vector4(cos, sin, 0, 0),
                                    new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
                                Matrix4x4 rotateXMat = new Matrix4x4(new Vector4(1, 0, 0, 0),
                                    new Vector4(0, sin, -cos, 0), new Vector4(0, cos, sin, 0),
                                    new Vector4(0, 0, 0, 1));
                                Matrix4x4 rotateYMat = new Matrix4x4(new Vector4(sin, 0, cos, 0),
                                    new Vector4(0, 1, 0, 0), new Vector4(-cos, 0, sin, 0), new Vector4(0, 0, 0, 1));
                                Vector3 targetDir = rotateXMat.MultiplyPoint(character.transform.up);
                                targetDir = rotateYMat.MultiplyPoint(targetDir);
                                //最后看向方向
                                targetDir = -rotateZMat.MultiplyPoint(targetDir);
                                slipTrue = targetDir;
                                moveSpeed = Vector3.zero;
                                isSlip = true;
                            }
                            moveSpeed += slipTrue * Time.fixedDeltaTime;
                            if (moveSpeed.sqrMagnitude > maxSlipSpeed * maxSlipSpeed)
                                moveSpeed = moveSpeed.normalized * maxSlipSpeed;
                            
                            preMove = moveSpeed * Time.fixedDeltaTime;
                            prePos = CharacterPos;
                            character.Move(preMove);
                            return false;
                        }
                    }
                    else
                        isGround = false;
                }
            }
            else isGround = false;

            if (!isGround)
            {
                RaycastHit hit;
                //有墙，可能可以爬
                if (Physics.Raycast(CharacterPos + Vector3.up * upClimbCheck + character.transform.forward * forwardClimbCheck,
                    Vector3.down, out hit, 0.1f, checkMask))
                {

                    moveSpeed = Vector3.zero;
                    //等待两秒
                    waitTime = 0.8f;
                    if (playerInfo != null)
                    {
                        playerInfo.allSystemPort.AnimaControl.StartRootAnimate();
                        Info.PlayerAction playerAction = playerInfo.characterAction as Info.PlayerAction;
                        playerAction.Climb = true;
                        character.Move(Vector3.up);
                        onWaitEnd = () =>
                        {
                            character.Move(character.transform.forward * 0.3f);
                            playerInfo.allSystemPort.AnimaControl.StopRootAnimate();
                            playerAction.Climb = false;
                        };
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// FixUpdate调用用的设置摄像机数据以及模型旋转数据
        /// </summary>
        private void SetCamera()
        {
            //检查是否有摄像机
            if (Camera.main != null)
            {
                //在地面，模型跟随摄像机
                if (isGround)
                {
                    Quaternion tem = Camera.main.transform.rotation;
                    float elurY = tem.eulerAngles.y;

                    character.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, character.transform.eulerAngles.y, 0),
                        Quaternion.Euler(0, elurY, 0), 10 * Time.fixedDeltaTime);
                    Camera.main.transform.rotation = tem;
                }
            }
        }

        /// <summary>
        /// 用来地面检测还有运动数值减少
        /// </summary>
        /// <param name="trueMove">实际移动值</param>
        /// <param name="matrix4X4">转化矩阵</param>
        private void WallCheckAndDecreaseMove(Vector3 trueMove, Matrix4x4 matrix4X4)
        {
            isWall = false;

            if (Mathf.Abs(trueMove.x) < Mathf.Abs(preMove.x) * 0.99f ||
            Mathf.Abs(trueMove.z) < Mathf.Abs(preMove.z) * 0.99f)
                if (!isSlip)
                    isWall = true;

            //减少速度，先放着先
            DecreaseSpeed(trueMove, preMove);

            //控制下落速度
            moveSpeed.y -= (isWall) ? gravity * Time.fixedDeltaTime * 0.8f : gravity * Time.fixedDeltaTime;
            //速度损耗
            moveSpeed = moveSpeed * (1 - friction * Time.fixedDeltaTime);

            prePos = character.transform.position;
            preMove = moveSpeed * Time.fixedDeltaTime;
            //trueMove = character.transform.forward * preMove.z + character.transform.right * preMove.x + character.transform.up * preMove.y;
            //preMove = trueMove;
            character.Move(preMove);
        }

        /// <summary>        /// 设置播放动画用的数据        /// </summary>
        private void SetAnimateValue()
        {
            if (playerInfo != null)
            {
                Info.PlayerAction playerAction = playerInfo.characterAction as Info.PlayerAction;
                if (playerAction != null)
                {
                    if (Mathf.Abs(moveSpeed.x) >= 0.1 || Mathf.Abs(moveSpeed.z) >= 0.1)
                        playerAction.Move = true;
                    else
                        playerAction.Move = false;
                }

                playerAction.Jump = !isGround;
            }
        }

        /// <summary>
        /// 进行移动数值的减少
        /// </summary>
        /// <param name="trueMove">实际移动值</param>
        /// <param name="preMove">理想移动值</param>
        /// <param name="matrix4X4">旋转矩阵</param>
        private void DecreaseSpeed(Vector3 trueMove, Vector3 preMove)
        {

            float decrease;
            if (preMove.x != 0)
            {
                decrease = Mathf.Abs(trueMove.x) / Mathf.Abs(preMove.x);
                decrease = decrease + Mathf.Lerp(0.2f, 0, decrease);
                moveSpeed.x *= decrease;
            }
            if (preMove.y != 0)
            {
                decrease = Mathf.Abs(trueMove.y) / Mathf.Abs(preMove.y);
                decrease = decrease + Mathf.Lerp(0.2f, 0, decrease);
                moveSpeed.y *= decrease;
            }
            if (preMove.z != 0)
            {
                decrease = Mathf.Abs(trueMove.z) / Mathf.Abs(preMove.z);
                decrease = decrease + Mathf.Lerp(0.2f, 0, decrease);
                moveSpeed.z *= decrease;
            }
        }

        /// <summary>
        /// 进行模型移动，传入数据horizontal值以及vertical值，进行对当前模型的移动速度修改，
        /// 同时传入最大速度，让移动速度的变化值可以时时变化
        /// </summary>
        /// <param name="horizontal">前后比例值-1至1</param>
        /// <param name="vertical">左右比例值-1至1</param>
        /// <param name="maxSpeed">最大速度</param>
        public virtual void Move(float horizontal, float vertical, float maxSpeed)
        {
            if (isGround)
            {
                moveSpeed.x = 0;
                moveSpeed.z = 0;
                if (Mathf.Abs(horizontal) < 0.05f) horizontal = 0;
                Vector3 temp = character.transform.right * Mathf.Lerp(-maxSpeed, maxSpeed, horizontal / 2.0f + 0.5f);
                //防止上下移动，移动只动Y轴
                temp.y = 0;
                moveSpeed += temp;
                if (Mathf.Abs(vertical) < 0.05f) vertical = 0;
                temp = character.transform.forward * Mathf.Lerp(-maxSpeed, maxSpeed, vertical / 2.0f + 0.5f);
                //防止上下移动，移动只动Y轴
                temp.y = 0;
                moveSpeed += temp;
            }
        }

        public void Rotate(float mouseX, float mouseY, float rotateSpeed)
        {
            if (Camera.main == null) return;
            //模型转Y轴，摄像机转x轴
            Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(mouseY * Time.deltaTime * rotateSpeed, 0, 0);

            if (isGround)
                character.transform.rotation = character.transform.rotation * Quaternion.Euler(0, mouseX * Time.deltaTime * rotateSpeed, 0);
            //空中，摄像机可以自由旋转
            else
            {
                Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(0, mouseX * Time.deltaTime * rotateSpeed, 0);
                Vector3 angle = Camera.main.transform.eulerAngles;
                angle.z = 0;
                Camera.main.transform.rotation = Quaternion.Euler(angle);
            }

        }

        /// <summary>
        /// 跳跃，只要在特殊情况才可以跳跃
        /// </summary>
        /// <param name="jumpForce">跳跃强度</param>
        public void Jump(float jumpForce)
        {
            if (isGround)
            {
                moveSpeed.y = jumpForce;
                preMove = Vector3.zero;
                return;
            }

            if (Camera.main == null) return;
            if (isWall)
            {
                Quaternion tem = Camera.main.transform.rotation;
                Vector3 elur = tem.eulerAngles;

                character.transform.rotation = Quaternion.Euler(0, elur.y, 0);

                Camera.main.transform.rotation = tem;
                moveSpeed = character.transform.forward * jumpForce;
                moveSpeed.y = jumpForce;
                preMove = Vector3.zero;
            }

        }
    }
}