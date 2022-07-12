
using UnityEngine;


namespace FireControl.AI { 
    public class SettingInfo : MonoBehaviour {
        private static SettingInfo instance;
        public static SettingInfo Instance
        {
            get {
                if(instance == null)
                {
                    GameObject go = GameObject.Find("SetEnemysMachine");
                    if(go == null)
                        go = new GameObject("SetEnemysMachine");
                    if(go.GetComponent<SettingInfo>() == null)
                        instance = go.AddComponent<SettingInfo>();
                }
                return instance;
            }
        }

        private void OnEnable()
        {
            if(instance == null)
                instance = this;
        }

        private void OnDisable()
        {
            instance = null;
        }

        public string settingPath = "EnD/n.ed";
        public EnemyCreateMachine createMachine;
    }

}