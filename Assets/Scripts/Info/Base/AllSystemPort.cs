
using UnityEngine;

namespace FireControl.Info
{
    /// <summary>
    /// 所有系统的对外接口，用来方便每一个系统之间的交流
    /// </summary>
    public struct AllSystemPort
    {
        private Transform oriObj;

        /// <summary>        
        /// 创建所有人物系统获取结构体，封装了数据获取方式以及存储，不用就不读，用了才读      
        /// </summary>
        /// <param name="transform">根据用的transform组件</param>
        public AllSystemPort(Transform transform)
        {
            oriObj = transform;
            skillManage = null;
            motor = null;
            animaControl = null;
        }

        private Skill.SkillManage skillManage;
        public Skill.SkillManage SkillManage
        {
            get
            {
                if (skillManage == null)
                    skillManage = oriObj.GetComponentInChildren<Skill.SkillManage>();
                return skillManage;
            }
        }

        private Motor.FirstPersonMotor motor;

        /// <summary>        /// 主角的引擎，方便外部控制        /// </summary>
        public Motor.FirstPersonMotor PlayerMotor
        {
            get
            {
                if(motor == null)
                    motor = oriObj.GetComponentInChildren<Motor.FirstPersonMotor>();
                return motor;
            }
        }

        private Anima.AnimaControl animaControl;
        /// <summary>        /// 对象的动画管理类        /// </summary>
        public Anima.AnimaControl AnimaControl
        {
            get
            {
                if(animaControl == null)
                    animaControl = oriObj.GetComponentInChildren<Anima.AnimaControl>();
                return animaControl;
            }
        }

    }
}

