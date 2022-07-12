using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.AI
{
    /// <summary>
    /// �����������������ǿ�����һ����Χʱ����й������ɣ�
    /// �����ǿ���ʱ���д������뿪ʱ����ɾ��
    /// </summary>
    public class EnemyCreateMachine : MonoBehaviour
    {
        /// <summary>        /// ��������·��        /// </summary>
        private string baseLoadPath = Application.streamingAssetsPath + "/AI/";
        private Transform playerPos;
        /// <summary>        /// �Ƿ��м��ع�����        /// </summary>
        private bool isLoad;
        public bool canSee;
        /// <summary>        /// �洢�ż��ص��ı��        /// </summary>
        private int loadIndex;
        /// <summary>        /// �洢���AI���ļ�λ������        /// </summary>
        public string FileName = "EnD/Text.ed";
        /// <summary>        /// ���ط�Χ        /// </summary>
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
        /// �������������е��ã���������ͬһ����
        /// ����Ƿ�Ҫ���ع������ɾ������
        /// </summary>
        public void OnFixedUpdate()
        {
            //���������Զ�����
            float now = (playerPos.position - transform.position).sqrMagnitude;
            if (now > loadSqrRange)
            {
                canSee = false;
                if (transform.childCount > 0)
                {
                    while (transform.childCount != 0)
                    {
                        //�������������û������֣����Թ�ȫ������
                        Destroy(transform.GetChild(0));
                        isLoad = false;
                    }
                    //��ֵΪ�գ���ʾ���¼���
                    loadIndex = 0;
                }
            }
            else
            {
                canSee = true;
                if (!isLoad)
                {
                    //���ɹ���
                    //���ݴ洢��ʽ��
                    //�����й�������(����)����һ��λ��x,y,z
                    //�ڶ��нǶ�x,y,z��������HP����4��SP��������attack
                    List<string> strs = Common.FileLoad.FileReadAndWrite.
                         ReadFileByAngleBrackets(baseLoadPath);
                    if (strs == null) return;
                    if (strs.Count == 0) { strs.Clear(); strs = null; return; }
                    for (; loadIndex < strs.Count;)
                    {
                        List<string> vs = ReadEnemyData(strs[loadIndex]);
                        //���ض���
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
                        //һ������ֻ֡ѭ��һ��
                        return;
                    }
                    isLoad = true;
                }
            }
        }

        /// <summary>
        /// ��ȡ��������ݣ��������ɹ���ʱ����
        /// </summary>
        /// <param name="oriStr">һ�������һϵ������</param>
        /// <returns>���ش洢��������һϵ�����ݵ�����</returns>
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