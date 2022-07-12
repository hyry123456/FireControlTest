
using UnityEngine;

/// <summary>
/// �ر����͵��࣬һ��ֻ������һЩ�ر������ʹ�õ���
/// </summary>
namespace FireControl.SpecialClass
{
    /// <summary>
    /// �����ͻᴫ�͵ķ����������������ǽ
    /// </summary>
    public class NearTransfer : MonoBehaviour
    {
        /// <summary>        /// ���͵���Ŀ��λ��        /// </summary>
        public Transform targetPos;

        /// <summary>        /// ���;���        /// </summary>
        public float distance = 16f;

        public GameObject player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// ����֡ˢ�£���Ϊ�ƶ�Ҳ������֡��
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