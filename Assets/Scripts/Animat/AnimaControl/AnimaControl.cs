
using UnityEngine;

namespace FireControl.Anima
{
    public class AnimaControl : MonoBehaviour
    {
        string preAnimaName = null;
        Animator animator;
        public Animator Animator
        {
            get
            {
                if (animator == null)
                    animator = GetComponentInChildren<Animator>();
                return animator;
            }
        }
        public AnimaNameBase animaNameBase;
        protected Info.CharacterInfo characterInfo;

        /// <summary>
        /// 该事件会在动画的指定时间被调用，用来执行特定的方法，
        /// 传入的参数是int，用来表示用来执行的方法，
        /// 比如攻击方法有好几类，但是大致技能是固定的，通过编号判断攻击类型
        /// </summary>
        public Common.Handler.HandlerList.ISetOneParam<int> choseActionByParam;

        public void Start()
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null) Debug.LogError("没有动画组件");
            characterInfo = GetComponentInParent<Info.CharacterInfo>();
            if(characterInfo == null) characterInfo = gameObject.AddComponent<Info.CharacterInfo>();
        }

        private void Update()
        {
            SetAnimaValue();
        }

        /// <summary>
        /// 开启角色移动跟随动画
        /// </summary>
        public void StartRootAnimate()
        {
            if(animator != null)
                animator.applyRootMotion = true;
        }

        public void StopRootAnimate()
        {
            if(animator != null)
                animator.applyRootMotion = false;
        }

        /// <summary>
        /// 通过名称播放动画
        /// </summary>
        /// <param name="aniName">播放的属性</param>
        protected void PlayAnimaByName(string aniName)
        {
            if (aniName == null) return;
                animator.SetBool(aniName, true);
            if(preAnimaName != null && !aniName.Equals(preAnimaName))
                animator.SetBool(preAnimaName, false);
            preAnimaName = aniName;
        }

        /// <summary>        /// 通过名称关闭某个动画        /// </summary>
        /// <param name="aniName">动画名称</param>
        protected void CloseAnimaByName(string aniName)
        {
            animator.SetBool(aniName, false);
        }

        protected virtual void SetAnimaValue()
        {
            //animator.SetInteger("Attack", characterInfo.characterAction.Attack);
            //animator.SetInteger("Hurt", characterInfo.characterAction.Hurt);
            //animator.SetInteger("Move", characterInfo.characterAction.Move);
        }

        /// <summary>
        /// 设置动画属性的值
        /// </summary>
        /// <param name="aniName">属性名称</param>
        /// <param name="value">修改成的值</param>
        protected void SetAnimaFloatByName(string aniName, float value)
        {
            animator.SetFloat(aniName, value);
        }

        

    }
}