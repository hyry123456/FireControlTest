using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Setting
{
    /// <summary>
    /// 装备设置界面的一个装备图标点击、加载案件
    /// </summary>
    public class EquipSettingItem : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>        /// 存储着这个图标的按钮        /// </summary>
        public Button button;
        [HideInInspector]
        public string m_equipName;
        [HideInInspector]
        public string m_EquipDescipe;
        private EquipSetting m_EquipSetting;
        void Start()
        {
            button = GetComponent<Button>();
        }

        /// <summary>
        /// 加载这个图标时调用，一半只有开始时调用，用来处理一些行为
        /// </summary>
        /// <param name="equipName"></param>
        /// <param name="equipDecispe"></param>
        /// <param name="pictureName"></param>
        /// <param name="equipSetting">管理对象，用来显示这个装备信息</param>
        public void LoadEquip(string equipName, string equipDecispe, string pictureName, EquipSetting equipSetting)
        {
            this.m_equipName = equipName;
            this.m_EquipDescipe = equipDecispe;
            this.m_EquipSetting = equipSetting;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_EquipSetting.ShowThisItem(this);
        }
    }
}