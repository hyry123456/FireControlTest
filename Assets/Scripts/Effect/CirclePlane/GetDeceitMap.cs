
using UnityEngine;

public class GetDeceitMap : MonoBehaviour
{
    private static int DeciitTexID = Shader.PropertyToID("_DeceitTex");
    private Camera myCam;

    /// <summary>    /// �����洢������Ҫ��Ⱦ��Ŀ��ƽ������    /// </summary>
    public Transform[] targetPlanes;
    /// <summary>    /// �����洢����������Ⱦ��Ŀ��ƽ��ľ������ƽ������    /// </summary>
    public int[] reflectPlanes;
    private Transform mainCam;

    private RenderTexture render;


    void Start()
    {
        mainCam = Camera.main.transform;
        gameObject.name = "DecitCam";
        myCam = gameObject.AddComponent<Camera>();
        myCam.CopyFrom(Camera.main);
        myCam.depth = -10;
        myCam.targetTexture = render;
    }

    private void FixedUpdate()
    {
        if (render != null)
            RenderTexture.ReleaseTemporary(render);
        render = RenderTexture.GetTemporary(myCam.pixelWidth, myCam.pixelHeight, 0, RenderTextureFormat.RGB565);
        myCam.targetTexture = render;
        //�ж��䵽�����ĸ�ƽ��
        int targetIndex = GetTragetPlane();

        int reflectIndex = reflectPlanes[targetIndex];

        myCam.transform.position = targetPlanes[reflectIndex].position + (mainCam.position - targetPlanes[targetIndex].position);

        myCam.Render();
        Shader.SetGlobalTexture(DeciitTexID, render);
    }

    private int GetTragetPlane()
    {
        Vector3 mainDir = mainCam.forward.normalized;
        int target = 0;
        float tempSize = -1;
        for(int i=0; i<targetPlanes.Length; i++)
        {
            Vector3 planeDir = (targetPlanes[i].transform.position - mainCam.position).normalized;
            float nowSize = Vector3.Dot(planeDir, mainDir);
            if(nowSize > tempSize)
            {
                target = i;
                tempSize = nowSize;
            }
        }
        return target;
    }
    
}
