
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace FireControl.AI
{
    public class SetEnemysInfoByMachine : Editor
    {
        public static Transform mechine;

        /// <summary>
        /// 创建一个游戏对象，用来存放用来生成的怪物生成器对象
        /// </summary>
        [MenuItem("MyProjectSetting/AI/CreateSettingObj")]
        public static void CreateSettingObj()
        {
            SettingInfo info = SettingInfo.Instance;
        }

        /// <summary>
        /// 简单的删除创建用的生成器对象
        /// </summary>
        [MenuItem("MyProjectSetting/AI/DeleteSettingObk")]
        public static void DeleteSettingObk()
        {
            GameObject go = new GameObject("SetEnemysMachine");
            if(go != null)
                DestroyImmediate(go);
        }

        /// <summary>
        /// 设置存储的信息，同时清空所有子物体
        /// </summary>
        [MenuItem("MyProjectSetting/AI/SetCreateMechineInfo")]
        public static void SetCreateMechineInfo()
        {
            GameObject go = GameObject.Find("SetEnemysMachine");
            //获取以及判断信息
            if(go == null){ Debug.LogError("没有创建设置信息的根据对象"); return;    }
            SettingInfo settingInfo = go.GetComponent<SettingInfo>();
            if(settingInfo == null){    Debug.LogError("这个对象没有根据的组件"); return ; }
            if(settingInfo.createMachine == null){  Debug.LogError("没有存放根据信息"); return; }

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
                //保存怪物的预制件路径名称
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

            //清除所有子对象
            while(settingInfo.createMachine.transform.childCount > 0)
                DestroyImmediate(settingInfo.createMachine.transform.GetChild(0).gameObject);

            Common.FileLoad.FileReadAndWrite.WriteFile(
            Application.streamingAssetsPath + "/AI/" + settingInfo.createMachine.FileName, saveStr);
        }

        /// <summary>
        /// 加载存储在这个文件中的所有怪物的情况，记得加载后删除
        /// </summary>
        [MenuItem("MyProjectSetting/AI/LoadCreateMechineEnemy")]
        public static void LoadCreateMechineEnemy()
        {
            GameObject go = GameObject.Find("SetEnemysMachine");
            //获取以及判断信息
            if (go == null) { Debug.LogError("没有创建设置信息的根据对象"); return; }
            SettingInfo settingInfo = go.GetComponent<SettingInfo>();
            if (settingInfo == null) { Debug.LogError("这个对象没有根据的组件"); return; }
            if (settingInfo.createMachine == null) { Debug.LogError("没有存放根据信息"); return; }
            //设置为设置对象的文本路径
            settingInfo.settingPath = settingInfo.createMachine.FileName;

            LoadEnemy(settingInfo.createMachine);
        }

        /// <summary>
        /// 加载所有的敌人
        /// </summary>
        private static void LoadEnemy(EnemyCreateMachine parent)
        {
            //生成怪物
            //数据存储格式：
            //第零行怪物类型(名称)，第一行位置x,y,z
            //第二行角度x,y,z，第三行HP，第4行SP，第五行attack
            List<string> strs = Common.FileLoad.FileReadAndWrite.
                    ReadFileByAngleBrackets(Application.streamingAssetsPath + "/AI/" + parent.FileName);
            if (strs == null) return;
            if (strs.Count == 0) { strs.Clear(); strs = null; return; }
            for (int i=0; i < strs.Count; i++)
            {
                List<string> vs = ReadEnemyData(strs[i]);
                Debug.Log(vs[0]);
                //加载对象
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
        /// 读取怪物的数据，用来生成怪物时调用
        /// </summary>
        /// <param name="oriStr">一个怪物的一系列数据</param>
        /// <returns>返回存储这个怪物的一系列数据的数组</returns>
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