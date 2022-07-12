using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 装备交互信息获取类，由装备类进行调用，用来显示新塞入背包的物品信息
    /// 同时对于一些获取物品信息进行显示，这个不确定是否需要添加
    /// </summary>
    public class EquipUIControl : UIUseBase
    {
        protected Text equipTex;
        protected GameObject background;
        public string backgroundChildName = "Image_ShowBackground";

        protected bool isShowInfo = false;
        protected override void Awake()
        {
            base.Awake();
            equipTex = GetComponentInChildren<Text>();
            background = transform.Find(backgroundChildName).gameObject;
            equipTex.gameObject.SetActive(false);
            background.SetActive(false);

        }
        /// <summary>
        /// 检查是否退出信息显示
        /// </summary>
        private void Update()
        {
            if (!isShowInfo) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FireControl.Interaction.InteractionControl.Instance.StopInteraction();
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 显示装备信息，这个只是暂时的，因为不确定之后需要添加其他内容的方式
        /// 比如之后需要添加图片什么的
        /// </summary>
        /// <param name="infoContent">装备信息描述</param>
        /// <param name="equipName">装备名称</param>
        public void ShowEquipInfo(string infoContent, string equipName)
        {
            FireControl.Interaction.InteractionControl.Instance.StartInteraction();
            equipTex.text = infoContent;
            background.SetActive(true);
            equipTex.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            equipTex.text = "";
            background.SetActive(false);
            equipTex.gameObject.SetActive(false);
        }

    }
}