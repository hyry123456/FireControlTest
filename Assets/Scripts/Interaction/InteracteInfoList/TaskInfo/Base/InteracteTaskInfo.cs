using System.Collections;


namespace FireControl.Interaction
{
    /// <summary>
    /// 交互任务信息类，当一个交互信息是任务类型时需要继承该类
    /// 同时设置交互类型是任务，方便调用，主要任务是对于
    /// </summary>
    public abstract class InteracteTaskInfo : InteractionInfo
    {
        protected int chapterID = 0;
        /// <summary>        /// 任务章节的ID，表示这个交互是发给这个章节的        /// </summary>
        public int ChapterID { 
            get { return chapterID; }
        }
    }
}