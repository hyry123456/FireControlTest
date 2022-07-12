

using System;
using UnityEngine;

namespace FireControl.Task
{
    public abstract class TaskPart
    {
        public string partName;
        public string partDescribe;

        /// <summary>
        /// ���������¼����ú���������������ã�һ���������ֶμ���������ã�
        /// Ҳ������TaskControl����BeginChapter���е�һ�μ��س���ʱ���ã�
        /// �ڶ��������Ѿ���ȡ�������н����ı��ļ���ȡʱ���������SetNowTaskPart���е���
        /// ���о����������½��л�ʱ������½ڵ�����
        /// </summary>
        /// <param name="chapter">�½�����</param>
        public abstract void EnterTaskEvent(ChapterTask chapter);
        /// <summary>
        /// �Ƿ���������������ڼ�������Ƿ���ɣ�һ��ֻ���ڴ���һ�����񽻻���ͬʱ�ý����Ķ�Ӧ�½�ID
        /// ���Ǹ�����ʱ�Ż����ʹ�ã�һ�㲻�õ����Ƿ�ύ������Ҳ���Ǳ��������������
        /// </summary>
        /// <param name="chapter">�½�����</param>
        /// <param name="interactionInfo">������Ϣ</param>
        /// <returns>�Ƿ��������</returns>
        public abstract bool IsCompleteTask(ChapterTask chapter, Interaction.InteractionInfo interactionInfo);
        /// <summary>
        /// �½��˳��¼�����IsCompleteTask�����������(true)����ChangeTask���ã�
        /// ������ʾ��С�ڽ�����������һС��
        /// </summary>
        /// <param name="chapter"></param>
        public abstract void ExitTaskEvent(ChapterTask chapter);

        /// <summary>
        /// һ����װ�õĺ��������ڶ���һЩ�ոջ�ȡ�����壬���н����������
        /// </summary>
        /// <param name="gameObject">��Ҫ������յ�����</param>
        public void DestoryObjAllInteracte(GameObject gameObject)
        {
            Interaction.InteractionInfo[] interactionInfos = gameObject.GetComponentsInParent<Interaction.InteractionInfo>();
            for(int i=0; i<interactionInfos.Length; i++)
            {
                GameObject.Destroy(interactionInfos[i]);
            }
        }

    }
}