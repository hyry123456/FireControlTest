
using UnityEngine;


namespace FireControl.Effect
{
    /// <summary>
    /// �������Ч�����࣬������Чʱֻ��Ҫ���������꣬Ȼ������Ч����
    /// ��Ҫע�����������Ƶ��Ƕ�̬��Ч������һ����Ч������Ҫ���ʹ��ʱ�ſ�������Ч����Ȼ��Ҫ���������
    /// </summary>
    public class WorldEffectControl : MonoBehaviour
    {
        private static WorldEffectControl instance;
        public static WorldEffectControl Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = GameObject.Find("WorldEffect");
                    if(go == null)
                    {
                        go = new GameObject("WorldEffect");
                        instance = go.AddComponent<WorldEffectControl>();
                    }
                    else
                    {
                        instance = go.GetComponent<WorldEffectControl>();
                    }
                }
                return instance;
            }
        }

        private WorldEffectBase nowEffect = null;

        private WorldEffectControl()
        {
        }

        private void Awake()
        {
            if(instance == null) instance = this;
        }

        public void SetNowEffect(WorldEffectBase worldEffect)
        {
            nowEffect = worldEffect;
            Interaction.InteractionControl.Instance.StartInteraction();
            nowEffect.OnBegin();
        }
        
        public void FixedUpdate()
        {
            if (nowEffect == null)
                return;
            if(nowEffect.OnFixedUpdate())
            {
                WorldEffectBase worldEffect = nowEffect;
                nowEffect = null;
                worldEffect.OnEnd();
            }
        }


    }
}