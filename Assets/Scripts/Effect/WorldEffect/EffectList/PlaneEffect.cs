
using UnityEngine;

namespace FireControl.Effect
{
    /// <summary>
    /// 一个的平面特效类使用
    /// </summary>
    public class PlaneEffect : WorldEffectBase
    {

        float nowTime = 0;
        Vector3 targetPos;
        GameObject go;
        public PlaneEffect(Vector3 target)
        {
            targetPos = target;
        }
        Material setMat;
        public override void OnBegin()
        {
            if(setMat == null)
            {
                go = GameObject.Instantiate(Resources.Load<GameObject>("Effect/PlaneEffectObj"),
                    WorldEffectControl.Instance.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(7, 7, 7);
                go.transform.localRotation = Quaternion.identity;
                setMat = go.GetComponent<MeshRenderer>().material;
            }
            Vector4 targetPos4 = targetPos;
            targetPos4.w = 1;
            setMat.SetVector("_TargetPos", targetPos4);
            setMat.SetFloat("_NowTime", 0);
            nowTime = 0;
        }

        public override void OnEnd()
        {
            base.OnEnd();
            GameObject.Destroy(go);
            nowTime = 0;
            setMat = null;
        }

        public override bool OnFixedUpdate()
        {
            setMat.SetFloat("_NowTime", nowTime);
            nowTime += Time.fixedDeltaTime * 0.1f ;
            Debug.Log("PlaneEffect 播放中");
            if (nowTime > 1.5f)
                return true;
            return false;
        }
    }
}