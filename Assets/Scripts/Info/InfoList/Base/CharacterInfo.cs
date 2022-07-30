
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireControl.Info
{
     [System.Serializable]
    public class CharacterInfo : MonoBehaviour
    {
        protected float hardTime = 0;
        List<float> statesTime = new List<float>();

        /// <summary>        /// 攻击力        /// </summary>
        public int attack = 1;

        public List<CharacterState> characterStates = new List<CharacterState>();

        public string characterName;
        protected int hp = 10;
        public int maxHP = 10;
        [HideInInspector]
        public int sp = 10;
        public int maxSP = 10;
        public List<string> skills;
        /// <summary>        /// 跑步速度        /// </summary>
        public float runSpeed = 10;
        /// <summary>        /// 行走速度        /// </summary>
        public float walkSpeed = 5;
        public float rotateSpeed = 10;
        /// <summary>        /// 可以看见距离的平方，主角就是锁定距离   /// </summary>
        public float seeSqrDistance = 100;
        /// <summary>        /// 外界检测用的状态值        /// </summary>
        public CharacterAction characterAction;

        /// <summary>        /// 用来获得模型的真实transform，因为人物模型的Transform经常不可确定        /// </summary>
        public virtual Transform truthTransform
        {
            get
            {
                return transform;
            }
        }

        /// <summary>        /// 实际的模型Rotation值        /// </summary>
        public virtual Quaternion truthRotaion
        {
            get
            {
                return transform.rotation;
            }
            set
            {
                transform.rotation = value;
            }
        }

        /// <summary>
        /// 本项目所有系统的对外接口获取类，获取的是每一个对象的系统管理类
        /// 这个类会自动封装获取方法，没有时就是没有挂
        /// </summary>
        public AllSystemPort allSystemPort;


        /// <summary>        /// 主角是否处于硬直状态，死亡的时候也返回true，
        /// 保证死亡动画正常播放，是就返回true        /// </summary>
        public bool IsHardStraight
        {
            get
            {
                if (hardTime > 0)
                    return true;
                return false;
            }
        }

        protected virtual void FixedUpdate()
        {
            //减少硬直
            //if (hardTime > 0)
            //{
            //    hardTime -= Time.fixedDeltaTime;
            //    //if (hardTime <= 0)
            //        //characterAction.Hurt = 0;
            //}

            //循环更新人物状态
            for (int i = 0; i < characterStates.Count; i++)
            {
                //当事件存在负数时，表示这个状态是位置状态
                if (statesTime[i] < 0)
                    continue;
                statesTime[i] -= Time.fixedDeltaTime;
                //这里是正常进入负数的情况，开始执行消除该状态
                if (statesTime[i] < 0)
                {
                    characterStates[i].ExitStateBehavior();
                    characterStates.RemoveAt(i);
                    i--;
                }
            }

        }

        /// <summary>        /// 初始化方法        /// </summary>
        protected virtual void OnEnable()
        {
            //启动任务系统
            Task.TaskControl task = Task.TaskControl.Instance;
            hp = maxHP;
            sp = maxSP;
            characterAction = new CharacterAction();
            allSystemPort = new AllSystemPort(transform);
        }

        virtual protected void OnCharacterDie()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 当受到伤害时调用，确定是否需要硬直等操作
        /// </summary>
        protected virtual void OnHurt()
        {
            //characterAction.Hurt = 1;
            //SetHardTime(1);
        }

        //模型准备用的函数块
        #region ModelReady

        /// <summary>        /// 转化模型的法线方向，方便描边，保证描边的合理性，
        /// 这里是扔到栈中执行的方法，不是外部调用用的        /// </summary>
        private IEnumerator ConvertNormal()
        {
            Mesh mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
            var averageNormalHash = new Dictionary<Vector3, Vector3>();
            //没有时加入，重复时求平均
            for (var j = 0; j < mesh.vertexCount; j++)
            {
                if (!averageNormalHash.ContainsKey(mesh.vertices[j]))
                {
                    averageNormalHash.Add(mesh.vertices[j], mesh.normals[j]);
                }
                else
                {
                    averageNormalHash[mesh.vertices[j]] =
                        (averageNormalHash[mesh.vertices[j]] + mesh.normals[j]).normalized;
                }
                yield return null;

            }


            var averageNormals = new Vector3[mesh.vertexCount];
            for (var j = 0; j < mesh.vertexCount; j++)
            {
                averageNormals[j] = averageNormalHash[mesh.vertices[j]];
                yield return null;
            }

            var tangents = new Vector4[mesh.vertexCount];
            for (var j = 0; j < mesh.vertexCount; j++)
            {
                tangents[j] = new Vector4(averageNormals[j].x, averageNormals[j].y, averageNormals[j].z, 0);
                yield return null;
            }


            var colors = new Color[mesh.vertexCount];
            for (var j = 0; j < mesh.vertexCount; j++)
            {
                colors[j] = mesh.tangents[j];
                yield return null;
            }
            //将法线平均数据放到切线数据中
            mesh.tangents = tangents;
            //之前的切线数据放到为顶点色数据
            mesh.colors = colors;
            Debug.Log("Convert finall");
        }

        /// <summary>
        /// 外部调用的准备转化法线的函数，这个函数在一些特殊模型的Enable中调用
        /// </summary>
        protected void ReadyConvertNormal()
        {
            StartCoroutine(ConvertNormal());
        }
        #endregion

        /// <summary>
        /// 当血量给改变时执行的方法,返回经过buff处理后的血量值
        /// </summary>
        protected virtual int OnHPChange(int changeSize, CharacterInfo info)
        {
            if (characterStates == null) return changeSize;
            for (int i = 0; i < characterStates.Count; i++)
            {
                changeSize = characterStates[i].OnHPChange(changeSize, info);
            }
            return changeSize;
        }

        /// <summary>       /// 设置硬直时间,设置为负数就是持续硬质        /// </summary>
        public void SetHardTime(float time)
        {
            hardTime = time;
        }

        /// <summary>
        /// 停止持续硬直，防止一直硬直
        /// </summary>
        public void StopHard()
        {
            hardTime = 0;
            //characterAction.Hurt = 0;
        }

        /// <summary>        /// 返回角色是否死亡        /// </summary>
        public bool isDie
        {
            get
            {
                if (hp <= 0)
                    return true;
                return false;
            }
        }

        /// <summary>        /// 改变血量        /// </summary>
        /// <param name="changeSize">改变的大小，注意减血要为负数</param>
        /// <param name="info">攻击我的人</param>
        public void ChangeHP(int changeSize, CharacterInfo info)
        {
            changeSize = OnHPChange(changeSize, info);
            hp += changeSize;
            Debug.Log("HP = " + hp);
            if (changeSize < 0)
                OnHurt();
            if (hp <= 0)
                OnCharacterDie();
        }


        /// <summary>        /// 改变法值        /// </summary>
        /// <param name="changeSize">改变的大小，注意减血要为负数</param>
        public void ChangeSP(int changeSize)
        {
            changeSize = OnSPChange(changeSize);
            sp += changeSize;
        }

        /// <summary>
        /// 当SP改变时执行的方法，用来增加或减少损耗的蓝
        /// </summary>
        /// <param name="changeSize">改变的大小</param>
        /// <returns>调整后的值</returns>
        public virtual int OnSPChange(int changeSize)
        {
            if (characterStates == null) return changeSize;
            for (int i = 0; i < characterStates.Count; i++)
            {
                changeSize = characterStates[i].OnSPChange(changeSize);
            }
            return changeSize;
        }


        /// <summary>        /// 对象的实例ID，用来作为对象的判定根据        /// </summary>
        public int InstanceID
        {
            get { return GetInstanceID(); }
        }

        /// <summary>
        /// 给角色添加状态
        /// </summary>
        /// <param name="time">状态时间，当时间小于0时就是永不停止</param>
        /// <param name="state">添加的状态</param>
        public void AddState(float time, CharacterState state)
        {
            characterStates.Add(state);
            state.EnterStateBehavior();
            statesTime.Add(time);
        }

        /// <summary>
        /// 移除某个状态，移除的根据是状态的内置ID，传入的对象只要是同一个
        /// 数据类型就会被移除，不需要一定是同一个对象
        /// </summary>
        /// <param name="state">需要移除的状态类型</param>
        public void ReMoveState(CharacterState state)
        {
            for(int i=0; i<characterStates.Count; i++)
            {
                if (state.Equals(characterStates[i]))
                {
                    characterStates.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 刷新数据时调用的方法，用来给任务或者怪物进行初始化时调用
        /// </summary>
        public void RefreshData()
        {
            //调用初始化方法
            OnEnable();
        }


    }
}