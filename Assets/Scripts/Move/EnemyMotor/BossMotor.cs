

using UnityEngine;

namespace FireControl.Motor
{
    /// <summary>
    /// Boss���ƶ��࣬�����ֹͣ��ֱ���ƶ����ɶ����������ƶ�������ֻ���Ʒ���
    /// </summary>
    public class BossMotor : EnemyMotor
    {
        /// <summary>
        /// Boss�ĸ��ݷ�����ת
        /// </summary>
        public override void MoveByDir(Vector3 targetDir, bool runOrWalk)
        {
            Vector3 resultantDir = GetForce(targetDir);
            resultantDir.y = 0;
            LookByVector(resultantDir);
        }

        /// <summary>
        ///  Boss�ĸ���λ����ת
        /// </summary>
        public override void MoveByPos(Vector3 target, bool runOrWalk)
        {
            Vector3 resultantDir = GetForce(target - transform.position);
            resultantDir.y = 0;
            LookByVector(resultantDir);
        }
    }
}