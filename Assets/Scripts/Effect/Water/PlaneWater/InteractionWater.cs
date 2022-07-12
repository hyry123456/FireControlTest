using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionWater : MonoBehaviour
{
    private Camera mainCamera;
    public RenderTexture PrevRT;
    public RenderTexture CurrentRT;
    private RenderTexture TempRT;
    public Shader ClickShader;
    public Shader SperaShader;
    private Material RippleMat;
    private Material DrawMat;

    public float DrawRadius = 0.01f;
    public int TextureSize = 512;
    void Start()
    {
        mainCamera = Camera.main;
        CurrentRT = CreateRT();
        PrevRT = CreateRT();
        TempRT = CreateRT();

        DrawMat = new Material(ClickShader);
        RippleMat = new Material(SperaShader);
        GetComponent<Renderer>().material.SetTexture("_InteractWaveTex", CurrentRT);
    }

    public RenderTexture CreateRT()
    {
        RenderTexture rt = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.RFloat);
        rt.Create();
        return rt;
    }

    private void DrawAt(float x, float y, float radius)
    {
        DrawMat.SetTexture("_SourceTex", CurrentRT);
        DrawMat.SetVector("_SourcePos", new Vector4(x, y, radius));

        Graphics.Blit(null, TempRT, DrawMat);

        RenderTexture rt = TempRT;
        TempRT = CurrentRT;
        CurrentRT = rt;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                DrawAt(hit.textureCoord.x, hit.textureCoord.y, DrawRadius);
            }
        }
        //设置上一帧图片
        RippleMat.SetTexture("_PrevRT", PrevRT);
        //当帧图片，如果发生了点击，点击的波纹生成在的纹理就是该纹理
        RippleMat.SetTexture("_CurrenRT", CurrentRT);
        //生成一个新的波纹图到该图片
        Graphics.Blit(null, TempRT, RippleMat);
        //将波纹图片渲染到上一帧
        Graphics.Blit(TempRT, PrevRT);
        //这么操作主要是问了保存当前的curr用来存储上一帧图片
        RenderTexture rt = PrevRT;
        PrevRT = CurrentRT;
        CurrentRT = rt;
    }
}