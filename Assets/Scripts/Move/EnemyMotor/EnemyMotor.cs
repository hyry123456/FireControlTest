
using UnityEngine;

namespace FireControl.Motor
{



    /// <summary>
    /// 该类是敌人移动的主要类，用来让敌人看向目标，有两个不同的方法方便旋转。
    /// </summary>
    public class EnemyMotor : Motor
    {
        /// <summary>        /// 探针起点,需要ProbePoint子物体        /// </summary>
        public Transform probePoint;
        /// <summary>        /// 寻路探针长度        /// </summary>
        public float probeLength = 2.5f;
        /// <summary>        /// layer选择器        /// </summary>
        public LayerMask layerMask;
        /// <summary>        /// 用于力的计算        /// </summary>
        private Vector3 expectForce;
        /// <summary>        /// 用于切换射线        /// </summary>
        private bool changeRaycastEuler;
        /// <summary>        /// 判定是否朝向target的冷却时间，用于减缓抖动        /// </summary>
        public float getForceCdTime = 0.5f;
        /// <summary>        /// 当前的getForceCdTime时间        /// </summary>
        private float getForceTime;

        [HideInInspector]
        public CharacterController characterController;

        protected override void Start()
        {
            base.Start();
            characterController = GetComponentInChildren<CharacterController>();
            probePoint = transform.Find("ProbePoint");
            if (probePoint == null) probePoint = transform;
        }

        public void FixedUpdate()
        {
            GroundCheck();
        }


        private Vector3 playerVelocity;
        public bool groundedPlayer;
        private float gravityValue = -9.81f * 2;
        /// <summary>
        /// 重力
        /// </summary>
        private void GroundCheck()
        {
            playerVelocity = new Vector3(0, playerVelocity.y, 0);
            Collider[] collisions = Physics.OverlapSphere((transform.position +
                characterController.center) - Vector3.up * (characterController.height / 2 + 0.1f) + 
                Vector3.up * characterController.radius,
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
        public void LookByVector(Vector3 dir)
        {
            dir.y = 0;
            characterInfo.truthRotaion = Quaternion.Lerp(characterInfo.truthRotaion, Quaternion.LookRotation(dir),
                characterInfo.rotateSpeed * Time.fixedDeltaTime);
        }

        public void LookByPos(Vector3 Pos)
        {
            Pos.y = transform.position.y;
            Quaternion dir = Quaternion.LookRotation(Pos - transform.position);
            //transform.rotation = Quaternion.Lerp(transform.rotation, dir, 0.1f);
            characterInfo.truthRotaion = Quaternion.Lerp(characterInfo.truthRotaion, dir, 0.1f);
        }

        /// <summary>
        /// 移动到目标点，但是速度需要进行控制，比如慢慢走之类的
        /// </summary>
        /// <param name="target">目标点位置，我们会生成一个到达目标点的位置</param>
        /// <param name="runOrWalk">行走还是跑步，true是run，false是行走</param>
        public virtual void MoveByPos(Vector3 target, bool runOrWalk)
        {
            Vector3 resultantDir = GetForce(target - characterInfo.truthTransform.position);
            resultantDir.y = 0; 
            LookByVector(resultantDir);
            if (runOrWalk)
                characterController.Move(resultantDir * Time.fixedDeltaTime * characterInfo.runSpeed);
            else
                characterController.Move(resultantDir * Time.fixedDeltaTime * characterInfo.walkSpeed);
        }

        /// <summary>
        /// 按照方向移动到目标点位置
        /// </summary>
        /// <param name="targetDir">目标的方向</param>
        /// <param name="runOrWalk">行走还是跑步</param>
        public virtual void MoveByDir(Vector3 targetDir, bool runOrWalk)
        {
            Vector3 resultantDir = GetForce(targetDir);
            resultantDir.y = 0;
            LookByVector(resultantDir);
            if (runOrWalk)
                characterController.Move(resultantDir * Time.fixedDeltaTime * characterInfo.runSpeed);
            else
                characterController.Move(resultantDir * Time.fixedDeltaTime * characterInfo.walkSpeed);
        }


        /// <summary>        /// 计算力        /// 用于MoveByPos中得到当前没有障碍的前进方向
        /// </summary>
        /// <param name="target">目标方向</param>
        protected Vector3 GetForce(Vector3 target)
        {
            RaycastHit hit;
            //选一条射线探测墙体
            if ((changeRaycastEuler && Physics.Raycast(probePoint.position + Quaternion.Euler(0, 90, 0) * 
                transform.forward * 0.5f,
                transform.forward, out hit, probeLength, layerMask)) ||
                (!changeRaycastEuler && Physics.Raycast(probePoint.position + Quaternion.Euler(0, -90, 0) * 
                transform.forward * 0.5f,
                transform.forward, out hit, probeLength, layerMask)))
            {
                //探测到墙体转向
                expectForce = Quaternion.Euler(0, (Random.Range(0f, 15f)+15f) *(changeRaycastEuler?-1:1), 0) * characterInfo.truthTransform.forward;
                //一段时间内不尝试朝向target
                getForceTime = getForceCdTime;
            }
            else
                //当前射线没打中，切换另一条
                changeRaycastEuler = !changeRaycastEuler;
            //计算朝向target冷却时间
            getForceTime -= Time.fixedDeltaTime;
            if (getForceTime < 0)
            {
                target = target.normalized;
                //如果target方向没有墙体则朝向target
                if (!(Physics.Raycast(probePoint.position + Quaternion.Euler(0, 90, 0) * target * 0.5f,
                   target, out hit, probeLength, layerMask) ||
                   Physics.Raycast(probePoint.position + Quaternion.Euler(0, -90, 0) * target * 0.5f,
                   target, out hit, probeLength, layerMask)))
                    expectForce = target;
            }
            return expectForce.normalized;
        }

    }
}