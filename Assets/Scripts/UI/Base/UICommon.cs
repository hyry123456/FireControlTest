
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{


    public class UICommon:MonoBehaviour
    {
        private static UICommon common;
        private UICommon(){ }

        /// <summary>
        /// UICommon����ĳ�ʼ�����Լ���ö���
        /// </summary>
        /// <returns>����һ��Ψһ����</returns>
        public static UICommon Initialize
        {
            get
            {
                if (common == null)
                {
                    //������Ϸ����
                    GameObject gameObject = new GameObject("UICommon");
                    common = gameObject.AddComponent<UICommon>();
                }
                return common;
            }
        }

        private void Start()
        {
            if(common == null)
                common = this.GetComponent<UICommon>();
        }

        /// <summary>
        /// ��ȡ��Ϸ����
        /// </summary>
        public GameObject GetGameObject(string name, UIControl uIControl)
        {
            GameObject temp;
            if (uIControl == null)
            {
                Debug.Log("û��UIControl");
                return null;
            }
            uIControl.UIObjectDictionary.TryGetValue(name, out temp);
            return temp;
        }

        /// <summary>
        /// ��ʾUI
        /// </summary>
        /// <param name="name">Ҫ��ʾ��UI������</param>
        /// <param name="control">UI�������</param>
        public void ShowUI(string name, UIControl control)
        {
            control.UIObjectDictionary[name].SetActive(true);
        }
        /// <summary>
        /// �ر�UI
        /// </summary>
        /// <param name="name">Ҫ�ر���ʾ��UI������</param>
        /// <param name="control">UI�������</param>
        public void CloseUI(string name, UIControl control)
        {
            GameObject gameObject;
            if(control.UIObjectDictionary.TryGetValue(name, out gameObject))
                gameObject.SetActive(false);
        }

        /// <summary>
        /// ����UI��ʾ���߹رգ����UI����ʾ״̬�͹رգ�����ǹر�״̬����ʾ
        /// </summary>
        public void SetUICloseOrClick(string name, UIControl control)
        {
            GameObject game = control.UIObjectDictionary[name];
            if(game == null)
            {
                Debug.Log("δ�ҵ�UI" + name);
                return;
            }
            if (game.activeSelf)
            {
                game.SetActive(false);
            }
            else
                game.SetActive(true);
        }

        /// <summary>
        /// ���ں������ɵ�UI����UIControl�й���ķ�������ֹ������ӵ�UI���ܽ��й���
        /// </summary>
        /// <param name="widgrt">UI������</param>
        /// <param name="control">control���</param>
        /// <param name="isShow">ȷ��������Ƿ���ʾ��������ӵ����岻���ܳ�ʼ����Ӱ���ˣ�
        /// ������ֱ��������ʾ�Լ�����</param>
        public void LateUIAddControl(UISceneWidgrt widgrt, UIControl control, bool isShow)
        {
            if (widgrt == null) return;
            ISceneClickHandler sceneClickHandler = transform.GetComponent<ISceneClickHandler>();
            control.UIObjectDictionary.Add(widgrt.name, widgrt.gameObject);
            control.AddSceneClick(sceneClickHandler.ScenePointClick);
            widgrt.gameObject.SetActive(isShow);
        }

        /// <summary>
        /// ����һЩ��Ҫ����ɾ���Ķ�̬UI����ɾ������
        /// </summary>
        /// <param name="widgrt">UI����</param>
        /// <param name="control">UI�������</param>
        public void DeleteUIInControl(UISceneWidgrt widgrt, UIControl control)
        {
            if (control.UIObjectDictionary.ContainsKey(widgrt.name))
            {
                control.UIObjectDictionary.Remove(widgrt.name);
                ISceneClickHandler pointClick = widgrt.GetComponent<ISceneClickHandler>();
                control.DeleteSceneClick(pointClick.ScenePointClick);
            }
        }

    }
}