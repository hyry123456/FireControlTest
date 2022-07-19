using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FireControl.Equip
{
    /// <summary>
    /// 该类是背包系统的重点，也是背包系统与外界交互的类，获取背包数据全部欧式通过该类的，这个类也是背包系统中最复杂的类，毕竟其做的交互多，不仅仅包括背包，连装备系统也是一同与整个类进行交互的。
    /// </summary>
    public class Bag : MonoBehaviour
    {
        /// <summary>        /// 存储着拥有的装备        /// </summary>
        public string bagSavePath = "/Equip/obtainEquip.equip";
        /// <summary>        /// 存储着正在装备的道具，也就是已经设置好的道具        /// </summary>
        public string fitOutSavePath = "/Equip/fitOutEquip.equip";
        private List<EquipBase> allEquip;
        [HideInInspector]
        public List<EquipBase> AllEquip
        {
            get { return allEquip; }
        }
        [HideInInspector]
        public int bagSize;
        [HideInInspector]
        public Armor helmetEquip;
        [HideInInspector]
        public Armor jacketEquip;
        [HideInInspector]
        public Armor trouserEquip;
        [HideInInspector]
        public Armor gloveEquip;
        [HideInInspector]
        public Armor shoeEquip;
        /// <summary>        /// 武器不能直接添加，因为需要设置位置        /// </summary>
        public Weapon weaponEquip;

        public Transform weaponPos;

        protected UI.UIExternalSimpleCommunicate communicate;

        private Info.PlayerInfo playerInfo;
        public Info.PlayerInfo PlayerInfo
        {
            get { 
                if(playerInfo = null)
                {
                    playerInfo = gameObject.GetComponent<Info.PlayerInfo>();
                }
                return playerInfo;
            }
        }

        public void Start()
        {
            bagSavePath = Application.streamingAssetsPath + bagSavePath;
            fitOutSavePath = Application.streamingAssetsPath + fitOutSavePath;
            if (weaponPos == null) weaponPos = transform;
            StartCoroutine(LoadBag());
            //GameObject go = GameObject.FindGameObjectWithTag("Player");
            //playerInfo = go.GetComponent<Info.PlayerInfo>();
            //先这么加载，将背包与主角在一起，需要的话可以变为单例模式，这样就不用每次启动都要加载了
            playerInfo = GetComponent<Info.PlayerInfo>();
        }

        /// <summary>
        /// 加载身上穿的装备的方法，用于在背包加载结束后进行身上穿的装备进行加载
        /// 数据存储格式：装备ID 装备位置编号
        /// </summary>
        public IEnumerator LoadEquip()
        {
            List<string> vs = Common.FileLoad.FileReadAndWrite.ReadFileByAngleBrackets(fitOutSavePath);
            for(int i=0; i<vs.Count; i++)
            {
                string[] strs = vs[i].Split(' ');
                int id = int.Parse(strs[0]);
                int type = int.Parse(strs[1]);
                for(int j=0; j<allEquip.Count; j++)
                {
                    if(allEquip[j].equipID == id)
                    {
                        switch (type) { 
                            case 0:
                                weaponEquip = allEquip[j] as Weapon;
                                Debug.Log("加载了武器" + weaponEquip.equipName);
                                break;
                            case 1:
                                Armor armor = allEquip[j] as Armor;
                                SetArmor(armor, false);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            yield return null;
        }

        /// <summary>
        /// 保存当前装备在身上的装备，好像有问题啊，这个函数
        /// </summary>
        public void SaveFitOytEquop()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
            stringBuilder.Append("<" + weaponEquip.equipID.ToString() + " " + ((int)weaponEquip.equipID).ToString() + ">\n");
            stringBuilder.Append("<" + helmetEquip.equipID.ToString() + " " + ((int)helmetEquip.equipID).ToString() + ">\n");
            stringBuilder.Append("<" + jacketEquip.equipID.ToString() + " " + ((int)jacketEquip.equipID).ToString() + ">\n");
            stringBuilder.Append("<" + trouserEquip.equipID.ToString() + " " + ((int)trouserEquip.equipID).ToString() + ">\n");
            stringBuilder.Append("<" + gloveEquip.equipID.ToString() + " " + ((int)gloveEquip.equipID).ToString() + ">\n");
            stringBuilder.Append("<" + shoeEquip.equipID.ToString() + " " + ((int)shoeEquip.equipID).ToString() + ">\n");
        }

        /// <summary>
        /// 重新设置武器以及其检测位置
        /// </summary>
        /// <param name="createPos">这个是创建位置，同时也是判断位置</param>
        public void ReSetWeapon(Transform createPos, Weapon weapon)
        {
            if (weapon == null)
                Debug.Log("Weapon is null");
            weaponEquip = weapon;
            weaponEquip.createPos = createPos;
        }

        /// <summary>        /// 添加装备进入背包中        /// </summary>
        /// <param name="equip">装备数据</param>
        /// <returns>是否添加成功</returns>
        public bool AddEquip(EquipBase equip)
        {
            int index = -1;
            for(int i=0; i < allEquip.Count; i++)
            {
                if(allEquip[i].equipID == equip.equipID)
                {
                    index = i;
                    break;
                }
            }
            //如果是新物体，简单加入即可
            if(index == -1)
            {
                if (allEquip.Count == bagSize)
                    return false;
                allEquip.Add(equip);
            }
            //旧的物体就进行数据刷新
            else
            {
                switch (equip.equipType)
                {
                    case EquipType.Weapon:
                        Weapon weapon = allEquip[index] as Weapon;
                        weapon.equipNeogor = weapon.maxNeogor;
                        break;
                    case EquipType.Armor:
                        Armor armor = allEquip[index] as Armor;
                        armor.equipNegor = armor.maxNegogor;
                        break;
                    case EquipType.Item:
                        Item item = allEquip[index] as Item;
                        item.count++;
                        break;
                }
            }
            SaveBag();
            return true;
        }

        /// <summary>
        /// 根据编号移除装备
        /// </summary>
        /// <param name="index">编号</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveEquipByIndex(int index)
        {
            if(index <0 || index > allEquip.Count)
                return false;
            allEquip.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// 根据物品ID进行物品移除
        /// </summary>
        /// <param name="equipID">根据的ID值</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveEquipByID(int equipID)
        {
            for(int i=0; i<allEquip.Count; i++)
            {
                if(allEquip[i].equipID == equipID)
                {
                    allEquip.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载背包，用于加载全部的装备数据
        /// 需要注意装备的存储格式：武器：类名 当前耐久
        /// 防具： 类名 当前耐久， 物品：类名 物品数量
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadBag()
        {
            List<string> strs = Common.FileLoad.FileReadAndWrite.ReadFileByAngleBrackets(bagSavePath);
            string targetPart = "FireControl.Equip.";
            Assembly assembly = Assembly.GetExecutingAssembly();
            EquipBase equipBase = null;
            allEquip = new List<EquipBase>();
            if (strs == null) yield break;
            for(int i=0; i<strs.Count; i++)
            {
                string[] vs = strs[i].Split(' ');
                string temp = targetPart + vs[0];
                equipBase = (EquipBase)assembly.CreateInstance(temp);
                int size;
                switch (equipBase.equipType)
                {
                    case EquipType.Weapon:
                        Weapon weapon = equipBase as Weapon;
                        weapon.createPos = this.weaponPos;
                        if (int.TryParse(vs[1], out size))
                            weapon.equipNeogor = size;
                        else weapon.equipNeogor = 0;
                        break;
                    case EquipType.Armor:
                        Armor armor = equipBase as Armor;
                        if (int.TryParse(vs[1], out size))
                            armor.equipNegor = size;
                        else armor.equipNegor = 0;
                        break;
                    case EquipType.Item:
                        Item item = equipBase as Item;
                        if (int.TryParse(vs[1], out size))
                            item.count = size;
                        else item.count = 1;
                        break;
                }
                allEquip.Add(equipBase);
                yield return null;
            }
            StartCoroutine(LoadEquip());
            yield break;
        }

        /// <summary>
        /// 保存背包数据，这里保存的只是背包列表的数据，不保存装备的数据
        /// </summary>
        public void SaveBag()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
            for(int i=0; i<allEquip.Count; i++)
            {
                int size;
                switch (allEquip[i].equipType)
                {
                    case EquipType.Weapon:
                        Weapon weapon = allEquip[i] as Weapon;
                        size = weapon.equipNeogor;
                        break;
                    case EquipType.Armor:
                        Armor armor = allEquip[i] as Armor;
                        size = armor.equipNegor;
                        break;
                    case EquipType.Item:
                        Item item = allEquip[i] as Item;
                        size = item.count;
                        break;
                    default:
                        size = 0;
                        break;
                }
                stringBuilder.Append("<" + allEquip[i].className + " " + size.ToString() + ">");
            }
            Common.FileLoad.FileReadAndWrite.WriteFile(bagSavePath, stringBuilder.ToString());
        }

        /// <summary>
        /// 执行武器行为
        /// </summary>
        public void RunWeapon()
        {
            //Debug.Log("运行武器行为");
            //没有武器以及武器没有耐久了就返回
            if (weaponEquip == null || weaponEquip.equipNeogor == 0)
            {
                return;
            }
            else
            {
                weaponEquip.WeaponBehavior(this);
            }

        }

        /// <summary>
        /// 设置装备防具,这个是一个内部封装类，根据传进来的防具类型判断这个防具
        /// 应该是放在哪里的防具
        /// </summary>
        /// <param name="armor">防具对象</param>
        protected void SetArmor(Armor armor, bool isNeedSave)
        {
            switch (armor.armorType)
            {
                case ArmorType.Helmet:
                    helmetEquip = armor;
                    break;
                case ArmorType.Jacket:
                    jacketEquip = armor;
                    break;
                case ArmorType.Trouser:
                    trouserEquip = armor;
                    break ;
                case ArmorType.Glove:
                    gloveEquip = armor;
                    break;
                case ArmorType.Shoe:
                    shoeEquip = armor;
                    break;
                default:
                    break;
            }
            if (isNeedSave)
                SaveFitOytEquop();
        }

        public void DeleteObtainEquipFile()
        {
            Common.FileLoad.FileReadAndWrite.WriteFile(bagSavePath, "");
            Common.FileLoad.FileReadAndWrite.WriteFile(fitOutSavePath, "");
        }
    }
}

