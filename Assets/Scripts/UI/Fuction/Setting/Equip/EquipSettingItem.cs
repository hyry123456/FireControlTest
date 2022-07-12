using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Setting
{
    /// <summary>
    /// װ�����ý����һ��װ��ͼ���������ذ���
    /// </summary>
    public class EquipSettingItem : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>        /// �洢�����ͼ��İ�ť        /// </summary>
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
        /// �������ͼ��ʱ���ã�һ��ֻ�п�ʼʱ���ã���������һЩ��Ϊ
        /// </summary>
        /// <param name="equipName"></param>
        /// <param name="equipDecispe"></param>
        /// <param name="pictureName"></param>
        /// <param name="equipSetting">�������������ʾ���װ����Ϣ</param>
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