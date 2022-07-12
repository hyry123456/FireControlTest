
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace FireControl.AI
{
    public class SetEnemysInfoByMachine : Editor
    {
        public static Transform mechine;

        /// <summary>
        /// ����һ����Ϸ������������������ɵĹ�������������
        /// </summary>
        [MenuItem("MyProjectSetting/AI/CreateSettingObj")]
        public static void CreateSettingObj()
        {
            SettingInfo info = SettingInfo.Instance;
        }

        /// <summary>
        /// �򵥵�ɾ�������õ�����������
        /// </summary>
        [MenuItem("MyProjectSetting/AI/DeleteSettingObk")]
        public static void DeleteSettingObk()
        {
            GameObject go = new GameObject("SetEnemysMachine");
            if(go != null)
                DestroyImmediate(go);
        }

        /// <summary>
        /// ���ô洢����Ϣ��ͬʱ�������������
        /// </summary>
        [MenuItem("MyProjectSetting/AI/SetCreateMechineInfo")]
        public static void SetCreateMechineInfo()
        {
            GameObject go = GameObject.Find("SetEnemysMachine");
            //��ȡ�Լ��ж���Ϣ
            if(go == null){ Debug.LogError("û�д���������Ϣ�ĸ��ݶ���"); return;    }
            SettingInfo settingInfo = go.GetComponent<SettingInfo>();
            if(settingInfo == null){    Debug.LogError("�������û�и��ݵ����"); return ; }
            if(settingInfo.createMachine == null){  Debug.LogError("û�д�Ÿ�����Ϣ"); return; }

            settingInfo.createMachine.FileName = settingInfo.settingPath;

            string saveStr = "";
            for(int i=0; i < settingInfo.createMachine.transform.childCount; i++)
            {
                saveStr += '<';
                GameObject obj = settingInfo.createMachine.transform.GetChild(i).gameObject;
                string name = "";
                for(int j=0; j<obj.name.Length; j++)
                {
                    if ((obj.name[j] >= 'a' && obj.name[j] <= 'z') || (obj.name[j] >= 'A' && obj.name[j] <= 'Z')
                        || (obj.name[j] >= '0' && obj.name[j] <= '9'))
                        name += obj.name[j];
                    else
                        break;
                }
                //��������Ԥ�Ƽ�·������
                saveStr += "[" + name + "]";
                saveStr += "[" + obj.transform.position.x.ToString() +"," +
                    obj.transform.position.y.ToString() + "," +
                    obj.transform.position.z.ToString() + "]";
                saveStr += "[" + obj.transform.eulerAngles.x.ToString() + "," +
                    obj.transform.eulerAngles.y.ToString() + "," +
                    obj.transform.eulerAngles.z.ToString() + "]";
                Info.CharacterInfo characterInfo = obj.GetComponent<Info.CharacterInfo>();

                saveStr += "[" + characterInfo.maxHP + "]";
                saveStr += "[" + characterInfo.maxSP + "]";
                saveStr += "[" + characterInfo.attack + "]";
                saveStr += ">\n";
            }

            //��������Ӷ���
            while(settingInfo.createMachine.transform.childCount > 0)
                DestroyImmediate(settingInfo.createMachine.transform.GetChild(0).gameObject);

            Common.FileLoad.FileReadAndWrite.WriteFile(
            Application.streamingAssetsPath + "/AI/" + settingInfo.createMachine.FileName, saveStr);
        }

        /// <summary>
        /// ���ش洢������ļ��е����й����������ǵü��غ�ɾ��
        /// </summary>
        [MenuItem("MyProjectSetting/AI/LoadCreateMechineEnemy")]
        public static void LoadCreateMechineEnemy()
        {
            GameObject go = GameObject.Find("SetEnemysMachine");
            //��ȡ�Լ��ж���Ϣ
            if (go == null) { Debug.LogError("û�д���������Ϣ�ĸ��ݶ���"); return; }
            SettingInfo settingInfo = go.GetComponent<SettingInfo>();
            if (settingInfo == null) { Debug.LogError("�������û�и��ݵ����"); return; }
            if (settingInfo.createMachine == null) { Debug.LogError("û�д�Ÿ�����Ϣ"); return; }
            //����Ϊ���ö�����ı�·��
            settingInfo.settingPath = settingInfo.createMachine.FileName;

            LoadEnemy(settingInfo.createMachine);
        }

        /// <summary>
        /// �������еĵ���
        /// </summary>
        private static void LoadEnemy(EnemyCreateMachine parent)
        {
            //���ɹ���
            //���ݴ洢��ʽ��
            //�����й�������(����)����һ��λ��x,y,z
            //�ڶ��нǶ�x,y,z��������HP����4��SP��������attack
            List<string> strs = Common.FileLoad.FileReadAndWrite.
                    ReadFileByAngleBrackets(Application.streamingAssetsPath + "/AI/" + parent.FileName);
            if (strs == null) return;
            if (strs.Count == 0) { strs.Clear(); strs = null; return; }
            for (int i=0; i < strs.Count; i++)
            {
                List<string> vs = ReadEnemyData(strs[i]);
                Debug.Log(vs[0]);
                //���ض���
                GameObject game = Instantiate(Resources.Load<GameObject>("Enemy/" 
                    + vs[0]), parent.transform);
                game.tag = "Enemy";

                Info.CharacterInfo info = game.GetComponent<Info.CharacterInfo>();
                string[] vs1 = vs[1].Split(',');
                string[] vs2 = vs[2].Split(',');
                game.transform.position = new Vector3(float.Parse(vs1[0]), float.Parse(vs1[1]), float.Parse(vs1[2]));
                game.transform.rotation = Quaternion.Euler(
                    new Vector3(float.Parse(vs2[0]), float.Parse(vs2[1]), float.Parse(vs2[2])));
                info.maxHP = int.Parse(vs[3]);
                info.maxSP = int.Parse(vs[4]);
                info.attack = int.Parse(vs[5]);
            }
        }

        /// <summary>
        /// ��ȡ��������ݣ��������ɹ���ʱ����
        /// </summary>
        /// <param name="oriStr">һ�������һϵ������</param>
        /// <returns>���ش洢��������һϵ�����ݵ�����</returns>
        private static List<string> ReadEnemyData(string oriStr)
        {
            List<string> re = new List<string>(6);
            for (int i = oriStr.IndexOf('['); i < oriStr.Length && i != -1;)
            {
                int i2 = oriStr.IndexOf(']', i);
                if ((i2 - i) <= 1) return re;
                re.Add(oriStr.Substring(i + 1, i2 - 1 - i));
                i = oriStr.IndexOf('[', i2);
            }
            return re;
        }
    }
}