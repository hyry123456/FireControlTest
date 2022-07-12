using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.AI
{
    /// <summary>
    /// 怪物生成器，当主角靠近到一定范围时会进行怪物生成，
    /// 在主角靠近时进行创建，离开时进行删除
    /// </summary>
    public class EnemyCreateMachine : MonoBehaviour
    {
        /// <summary>        /// 基本加载路径        /// </summary>
        private string baseLoadPath = Application.streamingAssetsPath + "/AI/";
        private Transform playerPos;
        /// <summary>        /// 是否有加载过怪物        /// </summary>
        private bool isLoad;
        public bool canSee;
        /// <summary>        /// 存储着加载到的编号        /// </summary>
        private int loadIndex;
        /// <summary>        /// 存储这个AI的文件位置名称        /// </summary>
        public string FileName = "EnD/Text.ed";
        /// <summary>        /// 加载范围        /// </summary>
        public float loadSqrRange = 1000000f;

        

        private void Start()
        {
            baseLoadPath = baseLoadPath + FileName;
            //playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            playerPos = Control.PlayerControl.Instance.transform;
            isLoad = false;
        }

        private void OnEnable()
        {
            loadIndex = 0;
        }

        /// <summary>
        /// 由人物管理类进行调用，用来方便同一管理
        /// 检查是否要加载怪物或者删除怪物
        /// </summary>
        public void OnFixedUpdate()
        {
            //加载完后就自动返回
            float now = (playerPos.position - transform.position).sqrMagnitude;
            if (now > loadSqrRange)
            {
                canSee = false;
                if (transform.childCount > 0)
                {
                    while (transform.childCount != 0)
                    {
                        //这种情况是主角没有清完怪，所以怪全部复活
                        Destroy(transform.GetChild(0));
                        isLoad = false;
                    }
                    //赋值为空，表示重新加载
                    loadIndex = 0;
                }
            }
            else
            {
                canSee = true;
                if (!isLoad)
                {
                    //生成怪物
                    //数据存储格式：
                    //第零行怪物类型(名称)，第一行位置x,y,z
                    //第二行角度x,y,z，第三行HP，第4行SP，第五行attack
                    List<string> strs = Common.FileLoad.FileReadAndWrite.
                         ReadFileByAngleBrackets(baseLoadPath);
                    if (strs == null) return;
                    if (strs.Count == 0) { strs.Clear(); strs = null; return; }
                    for (; loadIndex < strs.Count;)
                    {
                        List<string> vs = ReadEnemyData(strs[loadIndex]);
                        //加载对象
                        GameObject game = Instantiate(Resources.Load<GameObject>("Enemy/" + vs[0]), transform);
                        game.tag = "Enemy";

                        Info.CharacterInfo info = game.GetComponent<Info.CharacterInfo>();
                        string[] vs1 = vs[1].Split(',');
                        string[] vs2 = vs[2].Split(',');
                        game.transform.position = new Vector3(float.Parse(vs1[0]), float.Parse(vs1[1]), float.Parse(vs1[2]));
                        game.transform.rotation = Quaternion.Euler(
                            new Vector3(float.Parse(vs2[0]), float.Parse(vs2[1]), float.Parse(vs2[2])));
                        info.maxHP = int.Parse(vs[3]);
                        info.maxSP = int.Parse(vs[4]);
                        info.attack = int.Parse(vs[5]);
                        game.SetActive(false);
                        StartCoroutine(DelayEnable(game));
                        loadIndex++;
                        //一个物理帧只循环一次
                        return;
                    }
                    isLoad = true;
                }
            }
        }

        /// <summary>
        /// 读取怪物的数据，用来生成怪物时调用
        /// </summary>
        /// <param name="oriStr">一个怪物的一系列数据</param>
        /// <returns>返回存储这个怪物的一系列数据的数组</returns>
        private List<string> ReadEnemyData(string oriStr)
        {
            List<string> re = new List<string>(6);
            for(int i=oriStr.IndexOf('['); i<oriStr.Length && i != -1;)
            {
                int i2 = oriStr.IndexOf(']', i);
                if ((i2 - i) <= 1) return re;
                re.Add(oriStr.Substring(i+1, i2 - 1 - i));
                i = oriStr.IndexOf('[', i2);
            }
            return re;
        }

        private IEnumerator DelayEnable(GameObject game)
        {
            yield return new WaitForFixedUpdate();
            game.SetActive(true);
        }
    }
}