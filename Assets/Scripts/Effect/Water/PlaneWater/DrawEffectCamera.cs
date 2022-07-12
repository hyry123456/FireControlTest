using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawEffectCamera : MonoBehaviour
{
    private static int directSeeTexture = Shader.PropertyToID("_GlobalRefractionTex");
    private static int reflectTexture = Shader.PropertyToID("_GlobalReflectionTex");
    private static int hasReflectTexture = Shader.PropertyToID("_HasReflectTex");
    public RenderTexture directSeeTex;
    public RenderTexture reflectTex;
    public Camera drawCam;
    public LayerMask cullMask = -1;
    void Start()
    {
        
        drawCam = gameObject.AddComponent<Camera>();
        gameObject.AddComponent<Skybox>();
        drawCam.enabled = false;
        //将该对象保存到场景中，不要切换场景后就用不了了
        //gameObject.hideFlags = HideFlags.HideAndDontSave;
        SetEqualMode();
    }

    /// <summary>
    /// 初始化我的数据采集摄像机的值
    /// </summary>
    private void SetEqualMode()
    {
        drawCam.clearFlags = Camera.main.clearFlags;
        drawCam.backgroundColor = Camera.main.backgroundColor;
        if(drawCam.clearFlags == CameraClearFlags.Skybox)
        {
            Skybox sky = Camera.main.GetComponent<Skybox>();
            Skybox mysky = drawCam.GetComponent<Skybox>();

            if(!sky || !sky.material)
            {
                mysky.enabled = false;
            }
            else
            {
                mysky.enabled = true;
                mysky.material = sky.material;
            }
        }
        drawCam.farClipPlane = Camera.main.farClipPlane;
        drawCam.nearClipPlane = Camera.main.nearClipPlane;
        drawCam.orthographic = Camera.main.orthographic;
        drawCam.fieldOfView = Camera.main.fieldOfView;
        drawCam.aspect = Camera.main.aspect;
        drawCam.orthographicSize = Camera.main.orthographicSize;
        drawCam.cullingMask = ~(1 << 4) & cullMask.value;
    }

    private void FixedUpdate()
    {
        if(directSeeTex != null)
            RenderTexture.ReleaseTemporary(directSeeTex);
        if (reflectTex != null)
            RenderTexture.ReleaseTemporary(reflectTex);

        drawCam.transform.position = Camera.main.transform.position;
        drawCam.transform.rotation = Camera.main.transform.rotation;
        directSeeTex = RenderTexture.GetTemporary(Camera.main.pixelWidth, 
            Camera.main.pixelHeight, 16, RenderTextureFormat.RGB565);
        drawCam.targetTexture = directSeeTex;
        drawCam.Render();
        Shader.SetGlobalTexture(directSeeTexture, directSeeTex);

        RaycastHit hit;
        if (Physics.Raycast(drawCam.transform.position, Vector3.down, out hit))
        {
            drawCam.transform.position += (hit.point - drawCam.transform.position) * 2;
            reflectTex = RenderTexture.GetTemporary(Camera.main.pixelWidth,
                Camera.main.pixelHeight, 16, RenderTextureFormat.RGB565);
            drawCam.targetTexture = reflectTex;
            Vector3 euler = drawCam.transform.rotation.eulerAngles;
            drawCam.transform.rotation = Quaternion.Euler(-euler.x, euler.y, euler.z);
            drawCam.Render();
            Shader.SetGlobalTexture(reflectTexture, reflectTex);
            Shader.SetGlobalInt(hasReflectTexture, 1);
        }
        else
        {
            Shader.SetGlobalInt(hasReflectTexture, 0);
        }

    }
}
