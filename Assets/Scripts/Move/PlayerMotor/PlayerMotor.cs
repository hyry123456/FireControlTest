
using UnityEngine;

namespace FireControl.Motor
{
    public class PlayerMotor : Motor
    {
        [HideInInspector]
        public CharacterController characterController;

        protected override void Start()
        {
            base.Start();
            characterController = GetComponentInChildren<CharacterController>();
        }

        private void FixedUpdate()
        {
            GroundCheck();
        }

        /// <summary>
        /// 移动
        /// 每秒向前移动moveSpeed
        /// </summary>
        /// <param name="vertical">每秒向前移动moveSpeed</param>
        public void Move(float vertical, float horizontal)
        {
            if (vertical != 0 && horizontal != 0)
            {
                characterController.Move(transform.forward * characterInfo.runSpeed * vertical * Time.fixedDeltaTime / 1.7f);
                characterController.Move(transform.right * characterInfo.runSpeed * horizontal * Time.fixedDeltaTime / 1.7f);
            }
            else
            {
                characterController.Move(transform.forward * characterInfo.runSpeed * vertical * Time.fixedDeltaTime);
                characterController.Move(transform.right * characterInfo.runSpeed * horizontal * Time.fixedDeltaTime);
            }
            if (horizontal != 0 || vertical != 0)
            {
                //characterInfo.characterAction.Move = 1;
            }
            else {
                //characterInfo.characterAction.Move = 0; 
            }
        }

        /// <summary>
        /// 根据移动值进行跑步或走路
        /// </summary>
        /// <param name="value">是否移动</param>
        /// <param name="runOrWalk">true是跑步，false走路</param>
        //public void RunOrWalk(bool value, bool runOrWalk)
        //{

        //    //if(!value) characterInfo.characterAction.Move = 0;
        //    else
        //    {
        //        if (runOrWalk)
        //            //characterInfo.characterAction.Move = 3;
        //        else
        //            //characterInfo.characterAction.Move = 1;
        //    }

        //}

        private Vector3 playerVelocity;
        public bool groundedPlayer;
        private float jumpHeight = 1.0f;
        private float gravityValue = -9.81f * 2;
        private void GroundCheck()
        {
            playerVelocity = new Vector3(0, playerVelocity.y, 0);
            Info.PlayerInfo playerInfo = characterInfo as Info.PlayerInfo;
            Collider[] collisions = Physics.OverlapSphere(characterController.transform.position - 
                Vector3.up * (characterController.height / 2 + 0.1f) + Vector3.up * characterController.radius,
                            characterController.radius, 1 << LayerMask.NameToLayer("Map"));
            if (collisions.Length > 0)
            {
                groundedPlayer = true;
            }
            else
                groundedPlayer = false;

            if (groundedPlayer && playerVelocity.y <= 0)
            {
                playerVelocity.y = -1f;
            }
            if (!groundedPlayer)
                playerVelocity.y += gravityValue * Time.fixedDeltaTime;
            characterController.Move(playerVelocity * Time.fixedDeltaTime);
        }

        public void Jump(bool jump)
        {
            if (jump && !(Physics.OverlapSphere(transform.position - Vector3.up * (characterController.height / 2 + 0.2f)
                + Vector3.up * characterController.radius,
                            characterController.radius, 1 << LayerMask.NameToLayer("Map")).Length == 0))
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
        }

        public override void TransferToPos(Vector3 target)
        {
            characterController.Move(target - transform.position);
        }

        public override void MoveAndRotate(Vector3 rotateDir, float rotateSpeed, float moveSpeed)
        {
            if (rotateDir.normalized == transform.forward) return;
            Info.PlayerInfo playerInfo = characterInfo as Info.PlayerInfo;

            Vector3 targetDir = Vector3.Lerp(Quaternion.Euler(0, 0.1f, 0) *
                playerInfo.truthTransform.forward, rotateDir, Time.fixedDeltaTime * rotateSpeed).normalized;

            targetDir = playerInfo.truthTransform.position + targetDir;
            targetDir.y = playerInfo.truthTransform.position.y;

            playerInfo.truthTransform.LookAt(targetDir);
            characterController.Move(playerInfo.truthTransform.forward * moveSpeed * Time.fixedDeltaTime);
        }
    }
}