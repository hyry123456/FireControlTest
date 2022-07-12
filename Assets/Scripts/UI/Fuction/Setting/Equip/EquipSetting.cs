
using UnityEngine;
using UnityEngine.UI;

namespace UI.Setting
{
    /// <summary>
    /// װ�����ý��棬������ʾ������װ������
    /// </summary>
    public class EquipSetting : SettingBase
    {
        /// <summary>        /// ������������ŵĸ�����        /// </summary>
        Transform createObj;
        /// <summary>
        /// �����ĸ���ԴItem
        /// </summary>
        GameObject origentItem;
        public string origentitemPath= "UI/Button_EquipItem";
        /// <summary>        /// ��Ʒ��������ʾ        /// </summary>
        public string equipNameShowText = "Text_EquipName";
        /// <summary>        /// ��Ʒ��������ʾ�ı�        /// </summary>
        public string equipDescibeShowText = "Text_EquipDescibe";
        private Text equipName;
        private Text equipDescibe;
        protected override void Awake()
        {
            base.Awake();
            equipDescibe = transform.Find(equipDescibeShowText).GetComponent<Text>();
            equipName = transform.Find(equipNameShowText).GetComponent<Text>();
        }

        protected void Start()
        {
            createObj = Common.CommonFunction.FindChildInTransform("Content", transform);
            origentItem = Resources.Load<GameObject>(origentitemPath);
        }

        public override void BeginSetting()
        {
            if (isSetting) return;
            isSetting = true;
            gameObject.SetActive(true);
            FireControl.Equip.Bag bag = FireControl.Control.PlayerControl.Instance.gameObject.GetComponent<FireControl.Equip.Bag>();
            int size = bag.AllEquip.Count;
            for (int i = 0; i < size; i++)
            {
                GameObject game = Instantiate(origentItem, createObj);
                EquipSettingItem item = game.GetComponent<EquipSettingItem>();
                item.LoadEquip(bag.AllEquip[i].equipName, bag.AllEquip[i].equipIntroduce, "", this);
            }
        }

        public override void EndSetting()
        {
            gameObject.SetActive(false);
            isSetting = false;
            int size = createObj.childCount;
            for(int i=0; i<size; i++)
            {
                Destroy(createObj.GetChild(0).gameObject);
            }
        }

        /// <summary>
        /// ��ʾ���װ������Ϣ
        /// </summary>
        /// <param name="item">װ������</param>
        public void ShowThisItem(EquipSettingItem item)
        {
            equipDescibe.text = item.m_EquipDescipe;
            equipName.text = item.m_equipName;
        }

    }
}