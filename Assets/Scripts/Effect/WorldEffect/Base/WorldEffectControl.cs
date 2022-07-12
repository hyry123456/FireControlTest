
using UnityEngine;


namespace FireControl.Effect
{
    /// <summary>
    /// 世界的特效管理类，创造特效时只需要设置其坐标，然后开启特效即可
    /// 需要注意的是这个控制的是动态特效，就是一个特效可能需要多次使用时才开启的特效，不然不要用这个调用
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