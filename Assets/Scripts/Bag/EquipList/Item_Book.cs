using UnityEngine;


namespace FireControl.Equip {
    /// <summary>
    /// һ�������õ��鱾���洢�����ǵĹ�������
    /// </summary>
    public class Item_Book : Item
    {
        public override EquipReturn ItemUseFunction()
        {
            return null;
        }

        protected override void Initialize()
        {
            this.equipID = 1;
            this.equipType = EquipType.Item;
            this.equipName = "���ǵĵ���";
            this.equipIntroduce = "һ����¼������֮ǰ������飬�Լ��������";
            className = "Item_Book";
        }
    }
}