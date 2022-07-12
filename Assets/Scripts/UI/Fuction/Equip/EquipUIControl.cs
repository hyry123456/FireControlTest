using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// װ��������Ϣ��ȡ�࣬��װ������е��ã�������ʾ�����뱳������Ʒ��Ϣ
    /// ͬʱ����һЩ��ȡ��Ʒ��Ϣ������ʾ�������ȷ���Ƿ���Ҫ���
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
        /// ����Ƿ��˳���Ϣ��ʾ
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
        /// ��ʾװ����Ϣ�����ֻ����ʱ�ģ���Ϊ��ȷ��֮����Ҫ����������ݵķ�ʽ
        /// ����֮����Ҫ���ͼƬʲô��
        /// </summary>
        /// <param name="infoContent">װ����Ϣ����</param>
        /// <param name="equipName">װ������</param>
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