
using UnityEngine;
using FireControl.Info;

namespace FireControl.Motor
{
    /// <summary>
    /// 该类是移动的基本类，一些简单的物体移动都是依靠该类的。
    /// </summary>
    public class Motor : MonoBehaviour
    {
        [HideInInspector]
        public Info.CharacterInfo characterInfo;
        [HideInInspector]
        public Anima.AnimaControl animaControl;

        protected virtual void Start()
        {
            characterInfo = GetComponent<Info.CharacterInfo>();
            animaControl = GetComponent<Anima.AnimaControl>();
        }

        /// <summary>
        /// 移动与旋转
        /// 每秒顺时针旋转rotateSpeed*90°
        /// 每秒向前移动moveSpeed
        /// </summary>
        /// <param name="rotateSpeed">每秒顺时针速度</param>
        /// <param name="moveSpeed">每秒向前移动moveSpeed</param>
        /// <param name="rotateDir">旋转到的方向</param>
        public virtual void MoveAndRotate(Vector3 rotateDir, float rotateSpeed, float moveSpeed)
        {
            Vector3 targetDir = Vector3.Lerp(transform.forward, rotateDir, Time.fixedDeltaTime * rotateSpeed).normalized;
            targetDir = transform.position + targetDir;
            targetDir.y = transform.position.y;
            transform.LookAt(targetDir);
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 传送到目标点
        /// </summary>
        public virtual void TransferToPos(Vector3 target)
        {
            transform.Translate(target - transform.position);
        }
    }
}