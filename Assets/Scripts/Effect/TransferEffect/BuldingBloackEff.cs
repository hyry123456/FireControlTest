
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.Effect
{
    /// <summary>
    /// 第一个场景的测试特效
    /// </summary>
    public class BuldingBloackEff : MonoBehaviour
    {
        public Material setMat;
        public Color maxColor;
        public Color minColor;
        private Color nowColor;
        public float maxHeight;
        public float maxWidth;
        public float eleHeight = 0.13f;
        public float eleWidth = 0.22f;
        public Vector3 moveDir;
        private Queue<Rigidbody> rigidbodies;
        private GameObject block;
        public float nowTime;
        float allTime;
        

        private void Start()
        {
            //block = Resources.Load<GameObject>("Effect/BuildBlockUse");
            ////block.AddComponent<Rigidbody>();
            //ShowEffect(5);
            Vector3 temp = Camera.main.transform.position;
            nowTime = 0;
            setMat.SetVector("_TargetPos", new Vector4(temp.x,temp.y,temp.z, 1));
        }

        //public void ShowEffect(float time)
        //{
        //    nowTime = 0;
        //    allTime = time;
        //    int sizeW = (int)(maxWidth / eleWidth) + 1;
        //    int sizeH = (int)(maxHeight / eleHeight) + 1;
        //    rigidbodies = new Queue<Rigidbody>();
        //    Vector3 pos = transform.position;
        //    for(int i=0; i<sizeH; i++)
        //    {
        //        for(int j=0; j<sizeW; j++)
        //        {
        //            GameObject go = Instantiate(block);
        //            go.transform.forward = moveDir;
        //            go.transform.position = new Vector3(pos.x - maxWidth / 2 + eleWidth * j, pos.y - maxHeight / 2 + eleHeight * i, pos.z);
        //            rigidbodies.Enqueue(go.GetComponent<Rigidbody>());
        //            go.transform.parent = transform;
        //        }
        //    }
        //}

        private void FixedUpdate()
        {
            //if (nowTime < allTime)
            //{
            //    float size = nowTime / allTime;
            //    setMat.SetColor("_EmissionColor", Color.Lerp(minColor, maxColor, size));
            //    nowTime += Time.fixedDeltaTime;
            //    Debug.Log(size);
            //}
            //else
            //{
            //    //Test();
            //    StartCoroutine(Test());
            //}
            //setMat.SetColor("_EmissionColor", Color.Lerp(minColor, maxColor, nowTime));
            setMat.SetFloat("_NowTime", nowTime);
            nowTime += (float)Time.fixedDeltaTime * 0.05f;
        }

        private IEnumerator Test()
        {
            while(rigidbodies != null && rigidbodies.Count != 0)
            {
                Rigidbody rigidbody = rigidbodies.Dequeue();
                rigidbody.useGravity = true;
                rigidbody.AddForce(moveDir * 5);
                Common.ObjectPool.AutoDelete auto = rigidbody.gameObject.AddComponent<Common.ObjectPool.AutoDelete>();
                auto.survivalTime = 5f;
                auto.isDestory = true;
                yield return new WaitForSeconds(0.5f);
            }
            yield break;
            

        }

    }
}