using System.Collections;


namespace FireControl.Interaction
{
    /// <summary>
    /// ����������Ϣ�࣬��һ��������Ϣ����������ʱ��Ҫ�̳и���
    /// ͬʱ���ý������������񣬷�����ã���Ҫ�����Ƕ���
    /// </summary>
    public abstract class InteracteTaskInfo : InteractionInfo
    {
        protected int chapterID = 0;
        /// <summary>        /// �����½ڵ�ID����ʾ��������Ƿ�������½ڵ�        /// </summary>
        public int ChapterID { 
            get { return chapterID; }
        }
    }
}