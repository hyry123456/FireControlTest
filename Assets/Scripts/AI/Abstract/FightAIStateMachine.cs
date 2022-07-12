
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace FireControl.AI
{
    /// <summary>    /// 锁定用的结构体，我们锁定的是第一个对象    /// </summary>
    public class LockStruct
    {
        /// <summary>        /// 对象造成的伤害值        /// </summary>
        public int attack;

        public Info.CharacterInfo character;

        public LockStruct(Info.CharacterInfo character)
        {
            this.character = character;
            attack = 0;
        }
        public LockStruct(Info.CharacterInfo character, int attack)
        {
            this.character = character;
            this.attack = attack;
        }
    }

    /// <summary>
    /// 标准的AI战斗状态机，只用来攻击主角的状态机，需要注意的是怪物的生成由怪物管理器去生成，
    /// 这个状态机只完成一些统一需要的操作，需要存储地点之类的由状态自己完成
    /// 我的打算是怪物的状态机是有一套自己的信息存储以及读取方式，在开始需要加载是读取文件加载对象
    /// 而不是一开始直接定义好所有的模型，然后等主角来了后再启动运行
    /// </summary>
    public class FightAIStateMachine : MonoBehaviour
    {
        /// <summary>        /// 用来判断这个目标是否有加载过是否看的见，没有时如果进行看见的判断，
        /// 会进行一次获取，这样可以在同一物理帧时不会重复运算        /// </summary>
        private bool targetIsLoad = false;
        /// <summary>        /// 是否看的见目标，这个会根据当前目标进行判断        /// </summary>
        private bool isCanSee = false;
        /// <summary>        /// 锁定的目标列表，这里面的目标都有可能是我的锁定目标        /// </summary>
        protected List<LockStruct> lockList;

        /// <summary>
        /// 获得当前锁定目标
        /// </summary>
        public Info.CharacterInfo GetTarget
        {
            get
            {
                if (lockList == null || lockList.Count == 0)
                    return null;
                else return lockList[0].character;
            }
        }
        public StateBase state;
        public StateType type;
        private Info.EnemyInfo enemyInfo;
        protected Motor.EnemyMotor enemyMotor;
        /// <summary>        /// 状态列表，用来存储之前创建过的状态，避免重复创建        /// </summary>
        private List<StateBase> states;

        private Skill.SkillManage skillManage;
        public Skill.SkillManage SkillManage
        {
            get
            {
                if(skillManage == null)
                    skillManage = GetComponent<Skill.SkillManage>();
                return skillManage;
            }
        }

        public Motor.EnemyMotor EnemyMotor
        {
            get { 
                if(enemyMotor == null) enemyMotor = GetComponent<Motor.EnemyMotor>();
                return enemyMotor; 
            }
        }

        /// <summary>        /// 用来加载状态的文本路径        /// </summary>
        public string LoadStatePath = "/AI/StaLis/n.ai";
        /// <summary>        /// 可以看见的角度        /// </summary>
        public float seeAngle = 120;
        [HideInInspector]
        /// <summary>        /// 当前的目标方向，也就是敌人看向主角的方向,时时刷新        /// </summary>
        public Vector3 targetDir;
        [HideInInspector]
        /// <summary>        /// 目标的距离平方，时时刷新        /// </summary>
        public float targetSqrDistance;

        /// <summary>        /// 本人物的状态信息        /// </summary>
        public Info.EnemyInfo EnemyInfo
        {
            get { return enemyInfo; }
        }

        protected void Awake()
        {
            LoadStatePath = Application.streamingAssetsPath + LoadStatePath;
        }

        protected void OnEnable()
        {
            enemyInfo = GetComponent<Info.EnemyInfo>();
            lockList = new List<LockStruct>();
            //设定主角为第一目标值
            lockList.Add(new LockStruct(Control.PlayerControl.Instance.GetComponent<Info.CharacterInfo>()));
            states = new List<StateBase>();
            List<string> strs = Common.FileLoad.FileReadAndWrite.ReadFileByAngleBrackets(LoadStatePath);
            if (strs == null) Debug.LogError("文本有误");
            Assembly assembly = Assembly.GetExecutingAssembly();
            string targetPart = "FireControl.AI.";
            for (int i = 0; i < strs.Count; i++)
            {
                StateBase stateBase = (StateBase)assembly.CreateInstance(targetPart + strs[i]);
                states.Add(stateBase);
            }
            state = GetStateByType(StateType.Default);
            state.EnterState(this, null);
            skillManage = GetComponent<Skill.SkillManage>();
        }

        /// <summary>        /// 关闭时全部初始化，清除数据之类的        /// </summary>
        protected void OnDisable()
        {
            state = null;
            enemyInfo = null;
            lockList.Clear();
            lockList = null;
            if (states != null)
            {
                states.Clear();
                states = null;
            }
            skillManage = null;
        }

        /// <summary>
        /// 更新目标信息，用来作为主物体
        /// </summary>
        private void UpdateTargetInfo()
        {
            //逐帧消除，表示没有加载敌人信息
            targetIsLoad = false;

            //清除死去的目标
            for(int i=0; i < lockList.Count;)
            {
                if (lockList[i].character == null || lockList[i].character.isDie) lockList.RemoveAt(i);
                else i++;
            }
            //没有目标就是说明怪物打赢了
            if(lockList == null || lockList.Count == 0)
            {
                targetIsLoad = true;
                isCanSee = false;
                //赋值最大，表示直接死亡不可见
                targetSqrDistance = float.MaxValue;
                return;
            }
            targetDir = lockList[0].character.truthTransform.position - enemyInfo.truthTransform.position;
            targetSqrDistance = targetDir.sqrMagnitude;
            
            //排序目标
            for(int i=1; i<lockList.Count; i++)
            {
                //仅向前交换，多帧循环就有序了，这样可以保证单帧运算少
                if(lockList[i].attack > lockList[i - 1].attack)
                {
                    LockStruct lockStruct = lockList[i];
                    lockList[i] = lockList[i - 1];
                    lockList[i - 1] = lockStruct;
                }
            }
        }

        protected void FixedUpdate()
        {
            //硬直状态就退出
            if (enemyInfo.IsHardStraight) return;
            if (state == null) return;
            UpdateTargetInfo();
            if (state == null) return;
            StateBase stateBase = state.OnFixedUpdate();
            if(stateBase != null)
            {
                CheckState(stateBase);
                state.ExitState(null);
                state = stateBase;
                state.EnterState(this, null);
            }
            if (state != null)
                type = state.StateType;
            else Debug.Log("type is null");
        }
        


        /// <summary>        /// 返回是否有看到目标        /// </summary>
        public bool canSeeTarget
        {
            get
            {
                if (targetIsLoad)
                {
                    return isCanSee;
                }
                else
                {
                    targetIsLoad = true;
                    //在可见角度中
                    if (targetSqrDistance < EnemyInfo.seeSqrDistance
                        && Vector3.Angle(targetDir, transform.forward) < seeAngle) {
                        RaycastHit raycastHit;
                        if (Physics.Raycast(transform.position, targetDir, out raycastHit, enemyInfo.seeSqrDistance))
                            //如果被其他东西遮挡，设为不可见
                            if (raycastHit.transform.GetComponent<Info.CharacterInfo>() == null)
                            {
                                isCanSee = false;
                            }
                            else isCanSee = true;
                        else
                            isCanSee = true;
                    }
                    //不在可见范围
                    else
                        isCanSee = false;
                    return isCanSee;
                }
            }
        }

        /// <summary>
        /// 通过状态类型查找状态，因为一个状态机的同一状态一般只有一个
        /// </summary>
        /// <returns>返回值可能为空，表示没有该状态，需要进行创建</returns>
        public StateBase GetStateByType(StateType type)
        {
            if (states == null)
                return null;
            for(int i=0; i<states.Count; i++)
            {
                if (states[i] == null) Debug.Log("State is null");
                if(states[i].StateType == type)
                    return states[i];
            }
            return null;
        }

        /// <summary>
        /// 检查是否存在该状态，如果没有就在列表中加入该状态
        /// </summary>
        /// <param name="state">检查的状态</param>
        protected void CheckState(StateBase state)
        {
            if (states == null) return;
            for(int i=0; i<states.Count; i++)
            {
                if (states[i].StateType == state.StateType)
                    return;
            }
            states.Add(state);
        }

        /// <summary>
        /// 用来添加怪物AI的锁定对象，也就是怪物被打时调用的方法
        /// </summary>
        /// <param name="info">被打的人的信息</param>
        /// <param name="attack"></param>
        public void AddAttackInfo(Info.CharacterInfo info, int attack)
        {
            StateBase tempState = GetStateByType(StateType.Attack);
            //if (tempState == null) return;
            this.state.ExitState(null);
            state = tempState;
            if (tempState == null) { 
                Debug.Log("State is null");
                return;
            }
            tempState.EnterState(this, null);
            if (lockList == null) lockList = new List<LockStruct>();
            for(int i=0; i<lockList.Count; i++)
            {
                if(lockList[i].character.InstanceID == info.InstanceID)
                {
                    lockList[i].attack += attack;
                    return;
                }
            }
            lockList.Add(new LockStruct(info, attack));
        }
    }
}