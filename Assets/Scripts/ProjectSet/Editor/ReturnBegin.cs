using UnityEditor;

namespace FireControl.ProjectSetting
{
    /// <summary>
    /// ʹ�ñ༭�����г�ʼ������,�������ļ���ʼ��
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