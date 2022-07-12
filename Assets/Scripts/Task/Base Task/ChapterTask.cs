
using System.Collections;

namespace FireControl.Task
{
    public abstract class ChapterTask
    {
        /// <summary>        /// 章节名称，这个用于在任务保存时调用，也就是ChapterTask后面多出来的部分        /// </summary>
        public string chapterName;
        /// <summary>        /// 章节标题，这个才是游戏中用于显示的任务名称        /// </summary>
        public string chapterTitle;
        protected int taskPartSize;
        protected TaskPart part;
        public int chapterID;
        public int nowCompletePart;
        public string chapterSavePath;

        /// <summary>
        /// 检查小节完成情况，用于任务的时时检查，判断是否可以进入下一个任务状态
        /// </summary>
        /// <param name="info">交互信息</param>
        public abstract void CheckTask(Interaction.InteractionInfo info);
        /// <summary>
        /// 检查该章节是否可以启动
        /// </summary>
        public abstract void CheckChapter();
        /// <summary>  
        /// 在章节检查完毕后进行调用，也就是发现该任务可以进行加载后会调用
        /// 用来给任务触发行为做准备，也就是加载任务的触发条件以及角色等等
        /// </summary>
        protected abstract void LoadChapter();
        /// <summary>
        /// 改变任务小节时调用
        /// </summary>
        public abstract void ChangeTask();

        /// <summary>
        /// 当章节开启时调用的方法，也就是章节的准备方法，当该任务可以触发时调用
        /// </summary>
        public abstract void BeginChapter();

        /// <summary>
        /// 加载该任务的对于任务小结，当任务系统加载已经获取但是没有完成的任务时会调用
        /// 设置为抽象是因为不同章节需要加载的任务不同
        /// </summary>
        public abstract void SetNowTaskPart(int nowPart);

        /// <summary>
        /// 完成任务行为，需要注意的是该任务需要设置交互停止，表示任务交互结束
        /// 方便反复循环，达到模拟逐帧调用的效果，毕竟有时任务退出需要干很多事情
        /// </summary>
        /// <returns>协程</returns>
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