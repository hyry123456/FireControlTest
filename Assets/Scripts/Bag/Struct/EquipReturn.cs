
namespace FireControl.Equip
{
    /// <summary>
    /// 装备效果返回结构体，用来存储进过武器效果返回的伤害值以及特效，要注意的是特效可能为空，所以记得判断条件。
    /// </summary>

    public class EquipReturn
    {
        public int value;
        public Info.CharacterState addState;
    }
}

