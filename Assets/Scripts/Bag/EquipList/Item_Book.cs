using UnityEngine;


namespace FireControl.Equip {
    /// <summary>
    /// 一本测试用的书本，存储着主角的故事内容
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
            this.equipName = "主角的档案";
            this.equipIntroduce = "一本记录了主角之前任务的书，以及人物介绍";
            className = "Item_Book";
        }
    }
}