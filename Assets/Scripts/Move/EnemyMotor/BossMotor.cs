

using UnityEngine;

namespace FireControl.Motor
{
    /// <summary>
    /// Boss的移动类，这个类停止了直接移动，由动画来控制移动，代码只控制方向
    /// </summary>
    public class BossMotor : EnemyMotor
    {
        /// <summary>
        /// Boss的根据方向旋转
        /// </summary>
        public override void MoveByDir(Vector3 targetDir, bool runOrWalk)
        {
            Vector3 resultantDir = GetForce(targetDir);
            resultantDir.y = 0;
            LookByVector(resultantDir);
        }

        /// <summary>
        ///  Boss的根据位置旋转
        /// </summary>
        public override void MoveByPos(Vector3 target, bool runOrWalk)
        {
            Vector3 resultantDir = GetForce(target - transform.position);
            resultantDir.y = 0;
            LookByVector(resultantDir);
        }
    }
}