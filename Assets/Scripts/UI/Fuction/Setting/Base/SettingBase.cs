
using UnityEngine;

namespace UI.Setting
{
    public abstract class SettingBase : UIUseBase
    {
        protected bool isSetting;
        protected override void Awake()
        {
            base.Awake();
            isSetting = false;
        }

        /// <summary>
        /// EnableҲ����Ϊfalse����֤�����ҿ�����ֻ��BeginSetting�ŻῪ��
        /// </summary>
        private void OnEnable()
        {
            isSetting = false;   
        }

        /// <summary>
        /// �ر���ʾʱ�������ʶ����������Ѿ�ֹͣ
        /// </summary>
        private void OnDisable()
        {
            isSetting = false;
        }

        /// <summary>
        /// ������������ý��棬����������ý������Ϊ
        /// </summary>
        public abstract void BeginSetting();

        /// <summary>
        /// ֹͣ����¼������һ������¼������˳���ִ�У��л�ʱ����ִ��
        /// </summary>
        public abstract void EndSetting();
    }
}