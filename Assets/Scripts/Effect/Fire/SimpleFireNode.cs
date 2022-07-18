using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FireControl.Effect.Fire
{
    public class SimpleFireNode : FireRoutineBase
    {
        public override void RotinuFuction(int nodeCount)
        {
            burnRange += fireValue * Time.deltaTime * nodeCount * spreadSpeed;
            for(int i=0; i<transform.childCount; i++)
            {
                MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
                for(int j=0; j<meshRenderers.Length; j++)
                {
                    //»ðÑæ´óÐ¡ÉèÖÃ
                    meshRenderers[j].material.SetFloat("_BurnSize", fireValue);
                    //È¼ÉÕ·¶Î§ÉèÖÃ
                    meshRenderers[j].material.SetFloat("_BlendSize", burnRange);
                }
            }
        }

        protected override void Start()
        {
            base.Start();
        }


        public override void AddBurn(float fireValue, float distance)
        {
            //¼òµ¥È¼ÉÕµþ¼Ó
            this.fireValue += Time.deltaTime * distance * fireValue * fireAddSpeed;
            //ÏÞÖÆ·¶Î§
            this.fireValue = Mathf.Clamp01(this.fireValue);
        }
    }
}