
using UnityEngine;
using FireControl.Info;

namespace FireControl.Motor
{
    /// <summary>
    /// �������ƶ��Ļ����࣬һЩ�򵥵������ƶ�������������ġ�
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
        /// �ƶ�����ת
        /// ÿ��˳ʱ����תrotateSpeed*90��
        /// ÿ����ǰ�ƶ�moveSpeed
        /// </summary>
        /// <param name="rotateSpeed">ÿ��˳ʱ���ٶ�</param>
        /// <param name="moveSpeed">ÿ����ǰ�ƶ�moveSpeed</param>
        /// <param name="rotateDir">��ת���ķ���</param>
        public virtual void MoveAndRotate(Vector3 rotateDir, float rotateSpeed, float moveSpeed)
        {
            Vector3 targetDir = Vector3.Lerp(transform.forward, rotateDir, Time.fixedDeltaTime * rotateSpeed).normalized;
            targetDir = transform.position + targetDir;
            targetDir.y = transform.position.y;
            transform.LookAt(targetDir);
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// ���͵�Ŀ���
        /// </summary>
        public virtual void TransferToPos(Vector3 target)
        {
            transform.Translate(target - transform.position);
        }
    }
}