
using UnityEngine;

namespace UI
{
    /// <summary>
    /// UIϵͳ����ӿڣ�UIControl�ɵĶ���̫��һ�ˣ����ʺ�ֱ��������
    /// ������Ҫ��ȡuiControl��Ҫ��ȡUICommon̫�鷳������ֱ���������
    /// ����Ϊ��װ����������ʹ�ã���Ҫע�����UI�ڲ���
    /// </summary>
    public class UIExternalCommunicate : MonoBehaviour
    {

        /// <summary>
        /// ���ڻ�ȡ���������UI����Ա
        /// </summary>
        protected UIControl control;
        protected virtual void Start()
        {
            control = GetComponent<UIControl>();
        }


        /// <summary>
        /// ����UI��ʾ
        /// </summary>
        /// <param name="uiName">��ʾ��UI����</param>
        public void SetUIActive(string uiName)
        {
            if (control == null) return;
            UICommon.Initialize.ShowUI(uiName, control);
        }

        /// <summary>
        /// �ر�UI
        /// </summary>
        /// <param name="uiName">�رյ�UI����</param>
        public void CloseUI(string uiName)
        {
            UICommon.Initialize.CloseUI(uiName, control);
        }

    }
}