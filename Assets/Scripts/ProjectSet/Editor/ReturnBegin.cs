using UnityEditor;

namespace FireControl.ProjectSetting
{
    /// <summary>
    /// 使用编辑器进行初始化方法,将任务文件初始化
    /// </summary>
    public class ReturnBegin : Editor
    {

        [MenuItem("MyProjectSetting/Task/Return")]
        public static void ReturnTask()
        {
            Task.TaskControl.Instance.ReturnBegin();
            Equip.Bag bag = new Equip.Bag();
            bag.DeleteObtainEquipFile();
            Task.TaskControl.ReleaseInstance();
        }
    }
}