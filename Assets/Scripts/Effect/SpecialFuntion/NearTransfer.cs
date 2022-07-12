
using UnityEngine;

/// <summary>
/// 特别类型的类，一般只用来干一些特别事情才使用的类
/// </summary>
namespace FireControl.SpecialClass
{
    /// <summary>
    /// 靠近就会传送的方法，用来制作鬼打墙
    /// </summary>
    public class NearTransfer : MonoBehaviour
    {
        /// <summary>        /// 传送到的目标位置        /// </summary>
        public Transform targetPos;

        /// <summary>        /// 传送距离        /// </summary>
        public float distance = 16f;

        public GameObject player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// 物理帧刷新，因为移动也是物理帧的
        /// </summary>
        private void FixedUpdate()
        {
            if (player == null) return;
            float dis = (transform.position - player.transform.position).sqrMagnitude;
            Debug.Log("dis=" + dis + " distance=" + distance);
            if(dis <= distance)
            {
                Vector3 pre = player.transform.position;
                player.transform.position = targetPos.position + (player.transform.position - transform.position);
                Debug.Log(player.transform.position + " " + pre);
            }
        }
    }
}