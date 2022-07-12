
using System.Collections;

namespace FireControl.Task
{
    public abstract class ChapterTask
    {
        /// <summary>        /// �½����ƣ�������������񱣴�ʱ���ã�Ҳ����ChapterTask���������Ĳ���        /// </summary>
        public string chapterName;
        /// <summary>        /// �½ڱ��⣬���������Ϸ��������ʾ����������        /// </summary>
        public string chapterTitle;
        protected int taskPartSize;
        protected TaskPart part;
        public int chapterID;
        public int nowCompletePart;
        public string chapterSavePath;

        /// <summary>
        /// ���С�������������������ʱʱ��飬�ж��Ƿ���Խ�����һ������״̬
        /// </summary>
        /// <param name="info">������Ϣ</param>
        public abstract void CheckTask(Interaction.InteractionInfo info);
        /// <summary>
        /// �����½��Ƿ��������
        /// </summary>
        public abstract void CheckChapter();
        /// <summary>  
        /// ���½ڼ����Ϻ���е��ã�Ҳ���Ƿ��ָ�������Խ��м��غ�����
        /// ���������񴥷���Ϊ��׼����Ҳ���Ǽ�������Ĵ��������Լ���ɫ�ȵ�
        /// </summary>
        protected abstract void LoadChapter();
        /// <summary>
        /// �ı�����С��ʱ����
        /// </summary>
        public abstract void ChangeTask();

        /// <summary>
        /// ���½ڿ���ʱ���õķ�����Ҳ�����½ڵ�׼������������������Դ���ʱ����
        /// </summary>
        public abstract void BeginChapter();

        /// <summary>
        /// ���ظ�����Ķ�������С�ᣬ������ϵͳ�����Ѿ���ȡ����û����ɵ�����ʱ�����
        /// ����Ϊ��������Ϊ��ͬ�½���Ҫ���ص�����ͬ
        /// </summary>
        public abstract void SetNowTaskPart(int nowPart);

        /// <summary>
        /// ���������Ϊ����Ҫע����Ǹ�������Ҫ���ý���ֹͣ����ʾ���񽻻�����
        /// ���㷴��ѭ�����ﵽģ����֡���õ�Ч�����Ͼ���ʱ�����˳���Ҫ�ɺܶ�����
        /// </summary>
        /// <returns>Э��</returns>
        public abstract IEnumerator ExitChapter();

        public virtual string GetPartName()
        {
            return part.partName;
        }

        public virtual string GetPartDescribe()
        {
            return part.partDescribe;
        }
    }
}