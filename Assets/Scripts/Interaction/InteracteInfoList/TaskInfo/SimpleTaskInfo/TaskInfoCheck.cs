using System.Collections;

namespace FireControl.Interaction
{
    /// <summary>
    /// ���񽻻��ļ�ⰸ��������һ�������ֻ��Ҫ���ø��࣬Ȼ������һ��IDֵ������
    /// </summary>
    public class TaskInfoCheck : InteracteTaskInfo
    {
        //private string addTaskName;
        /// <summary>        /// һ�������¼��������Ը÷�������һЩ����չ        /// </summary>
        public Common.Handler.HandlerList.INonReturnAndNonParam nonReAndNonParam;

        public TaskInfoCheck(int chapterID)
        {
            this.chapterID = chapterID;
        }

        public override IEnumerator InteractionBehavior(InteractionControl interControl)
        {
            Task.TaskControl.Instance.CheckChapter(this);
            if (nonReAndNonParam != null)
                nonReAndNonParam();
            yield break;
        }

        protected override void OnEnable()
        {
            interactionType = InteractionType.Task;
        }

        /// <summary>
        /// �������ý����¼�Ϊ����Ľ����¼�ʱ��ʹ��AddComponent�������������캯����������
        /// ����ʹ����������������½�ID
        /// </summary>
        public void SetChapterID(int id)
        {
            chapterID = id;
        }
    }
}