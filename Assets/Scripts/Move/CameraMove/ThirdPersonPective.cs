
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Motor
{
    /// <summary>    /// 第三人称摄像机引擎    /// </summary>
    public class ThirdPersonPective : MonoBehaviour
    {
        public float rotateSpeed = 100f;
        /// <summary>
        /// 子物体摄像机
        /// </summary>
        public Camera playerCamera;
        /// <summary>
        /// 记录默认摄像机位置
        /// </summary>
        /// <summary>
        public Vector3 cameraPos;
        /// <summary>
        /// 记录默认摄像机距离玩家距离
        /// </summary>
        public float normalDis;
        /// <summary>
        /// 用于记录玩家位置，使摄像机跟随
        /// </summary>
        public Vector3 playerPosition;
        /// <summary>
        /// 记录射线击中的Ground物体的点位置
        /// </summary>
        public Vector3 targetPos;

        public LayerMask hideMask;
        public LayerMask ignoreMask;
        public Queue<Renderer> renderer1;
        public RaycastHit[] hit;



        protected virtual void Start()
        {
            //获得view下的子物体PlayerCamera
            playerCamera = GetComponentInChildren<Camera>();
            //初始化cameraPos
            cameraPos = playerCamera.transform.localPosition;
            normalDis = cameraPos.magnitude;
            renderer1 = new Queue<Renderer>();
        }

        protected virtual void Update()
        {
            NormalView();
        }

        private void FixedUpdate()
        {
            FlowPlayer((Control.PlayerControl.Instance.PlayerInfo).truthTransform);
        }

        /// <summary>
        /// 跟随主角
        /// </summary>
        /// <param name="tf">主角的tf</param>
        public void FlowPlayer(Transform tf)
        {
            //水平位移不使用Lerp，以防摄像机跟不上物体而导致画面抖动
            playerPosition = tf.position;
            playerPosition.y = this.transform.position.y;
            this.transform.Translate(playerPosition - this.transform.position, Space.World);
            //垂直方向上使用lerp使垂直移动视角时更自然
            playerPosition.y = tf.position.y;
            this.transform.Translate(Vector3.Lerp(
                this.transform.position, playerPosition, 0.1f
                ) - this.transform.position, Space.World);
        }

        /// <summary>
        /// 控制镜头方向跟随鼠标旋转
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rotateSpeed"></param>
        public void ViewRotate(float x, float y)
        {
            if (this.transform.forward.y > 0)
            {
                if (this.transform.forward.y < 0.9 || -y < 0)
                    this.transform.Rotate(y * rotateSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                if (this.transform.forward.y > -0.9 || -y > 0)
                    this.transform.Rotate(y * rotateSpeed * Time.deltaTime, 0, 0);
            }
            this.transform.Rotate(0, x * rotateSpeed * Time.deltaTime, 0, Space.World);
        }

        /// <summary>
        /// 用于找到每个敌人，为星哥的敌人快速搜索列表
        /// </summary>
        public GameObject enemys;
        /// <summary>
        /// 最近目标，也是当前锁定目标
        /// </summary>
        public Transform minTarget;
        /// <summary>
        /// 记录最短距离，用于找出最近目标
        /// </summary>
        public float minLength;
        /// <summary>
        /// 可以被锁定的敌人的距离
        /// </summary>
        public float maxLockLength;
        private float rotateTime;
        /// <summary>
        /// 锁定视角
        /// 找到离屏幕中心点最近、距离小于maxLockLength的单位，然后LookAt
        /// </summary>
        /// <param name="mouse2Down">鼠标中键的按下</param>
        /// <returns>返回是否找到人</returns>
        public bool ViewLock(bool mouse2Down)
        {
            if(Camera.main == null) return false;
            //锁定中
            if (!mouse2Down && minTarget != null)
            {
                rotateTime += Time.deltaTime;
                if (rotateTime > 0.1f)
                    transform.LookAt(minTarget.position);
                else
                {
                    Quaternion dir = Quaternion.LookRotation(minTarget.position - transform.position);
                    this.transform.rotation = Quaternion.Lerp(transform.rotation, dir, 0.1f);
                }
                if ((minTarget.position - transform.position).sqrMagnitude < maxLockLength * maxLockLength)
                    return true;
                else
                    return false;
            }
            //开始锁定，寻找目标
            Vector2 centerPoint = new Vector2(Screen.width/2, Screen.height/2);
            Vector2 thisV2;
            minTarget = null;
            Transform thisTarget = null;
            minLength = Screen.width * Screen.width + Screen.height * Screen.height;

            //for (int i = 0; i < enemys.transform.childCount; i++)
            //    for (int ii = 0; ii < enemys.transform.GetChild(i).childCount; ii++)
            //    {
            //        thisTarget = enemys.transform.GetChild(i).GetChild(ii);
            //        if((thisTarget.position - transform.position).sqrMagnitude < maxLockLength * maxLockLength)
            //        {
            //            thisV2 = Camera.main.WorldToScreenPoint(thisTarget.position);
            //            if (thisV2.x > 0 && thisV2.x < Screen.width && thisV2.y > 0 && thisV2.y < Screen.height)
            //            if((thisV2 - centerPoint).sqrMagnitude < minLength)
            //            {
            //                minLength = (thisV2 - centerPoint).sqrMagnitude;
            //                minTarget = thisTarget;
            //                rotateTime = 0;
            //            }
            //        }
            //    }
            List<Info.EnemyInfo> enemies = Common.CharacterFouction.GetAllEnemy();
            for(int i=0; i< enemies.Count; i++)
            {
                thisTarget = enemies[i].transform;
                if ((thisTarget.position - transform.position).sqrMagnitude < maxLockLength * maxLockLength)
                {
                    thisV2 = Camera.main.WorldToScreenPoint(thisTarget.position);
                    if (thisV2.x > 0 && thisV2.x < Screen.width && thisV2.y > 0 && thisV2.y < Screen.height)
                        if ((thisV2 - centerPoint).sqrMagnitude < minLength)
                        {
                            minLength = (thisV2 - centerPoint).sqrMagnitude;
                            minTarget = thisTarget;
                            rotateTime = 0;
                        }
                }

            }
            enemies.Clear();
            enemies = null;
            if (minTarget == null)
            {
                minTarget = null;
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 鼠标滚轮切换选中对象
        /// </summary>
        public void ChangeLock(float mouseScrollWheel)
        {
            if (mouseScrollWheel == 0) return;
            //切换锁定，寻找目标
            Vector2 centerPoint = Camera.main.WorldToScreenPoint(minTarget.position);
            Vector2 thisV2;
            Transform thisTarget = null;
            Transform lastTarget = minTarget;
            minLength = Screen.width * Screen.width + Screen.height * Screen.height;
            List<Info.EnemyInfo> enemies = Common.CharacterFouction.GetAllEnemy();
            for (int i = 0; i < enemies.Count; i++)
            {
                thisTarget = enemies[i].transform;
                if (lastTarget == thisTarget) continue;
                if ((thisTarget.position - transform.position).sqrMagnitude < maxLockLength * maxLockLength)
                {
                    thisV2 = Camera.main.WorldToScreenPoint(thisTarget.position);
                    if ((centerPoint.x - thisV2.x) * mouseScrollWheel < 0) continue;
                    if ((thisV2 - centerPoint).sqrMagnitude < minLength)
                    {
                        minLength = (thisV2 - centerPoint).sqrMagnitude;
                        minTarget = thisTarget; rotateTime = 0.5f;
                        rotateTime = 0;
                    }
                }


            }
            enemies.Clear();
            enemies = null;

        }

        /// <summary>
        /// 正常的摄像机
        /// 人物后脑勺有Ground层的物体时时拉近摄像机
        /// 人物后脑勺有Transparent层的物体时将其设为半透明
        /// </summary>
        private void NormalView()
        {
            //击中的物体
            hit = null;
            hit = Physics.RaycastAll(this.transform.position,
                playerCamera.transform.position - this.transform.position,
                normalDis, ignoreMask|hideMask);

            //重置摄像机位置
            targetPos = this.transform.TransformPoint(cameraPos);
            //重置所有透明材质
            while (renderer1.Count != 0)
            {
                renderer1.Dequeue().material.SetFloat("_TransformSize", 1f);
            }

            //当有物体被击中时
            if (hit != null)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    //判断被击中物体类型
                    if (((1 << hit[i].collider.gameObject.layer) & hideMask) > 0)
                    {
                        targetPos = hit[i].point;
                    }
                    if (((1 << hit[i].collider.gameObject.layer) & ignoreMask) > 0) 
                    {
                        print("A");
                        renderer1.Enqueue(hit[i].collider.GetComponent<Renderer>());
                        renderer1.Peek().material.SetFloat("_TransformSize", 0.7f);
                    }
                }
            }

            //计算摄像机与被击中点之间距离、方向，并移动摄像机
            Vector3 v = targetPos - playerCamera.transform.position;
            if (v.sqrMagnitude > 0)
            {
                playerCamera.transform.Translate(v, Space.World);
            }
        }
    }
}
