
using UnityEngine;

namespace FireControl.Anima
{
    public class AnimaEvent : MonoBehaviour
    {
        public Transform createPos;
        public GameObject createObj;
        public float objSurvivalTime;
        private AnimaControl animaControl;

        private void Start()
        {
               animaControl = GetComponentInParent<AnimaControl>();
        }


        public void UseSkillByIndex(int index)
        {
            if (animaControl == null)
            {
                Debug.Log("没有control");
            }
            else
            {
                if (animaControl.choseActionByParam == null)
                {
                    Debug.Log("没有方法");
                }
            }

            if (animaControl != null && animaControl.choseActionByParam != null)
            {
                animaControl.choseActionByParam(index);
            }
        }


    }
}