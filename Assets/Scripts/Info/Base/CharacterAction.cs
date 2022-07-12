namespace FireControl.Info
{
    [System.Serializable]
    public class CharacterAction
    {
        /// <summary>        /// 移动值，表示是否移动       /// </summary>
        public bool Move = false;
    }

    [System.Serializable]
    public class PlayerAction : CharacterAction
    {
        public bool Climb = false;
        public bool Jump = false;
    }

    /// <summary>
    /// 给Boss用的行为记录，用来标识行为
    /// </summary>
    [System.Serializable]
    public class BossAction : CharacterAction
    {

        /// <summary>        /// 是否有锁定主角        /// </summary>
        public bool IsLock;
    }
}