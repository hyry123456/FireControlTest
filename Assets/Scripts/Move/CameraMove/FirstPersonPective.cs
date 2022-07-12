
using UnityEngine;

namespace FireControl.Motor
{
    /// <summary>    /// 摄像机引擎    /// </summary>
    public class FirstPersonPective : MonoBehaviour
    {
        public float rotateSpeed = 10f;


        /// <summary>
        /// 摄像机选择方法，需要注意的是该方法一定要在Update调用，因为Mouse检测是Update的
        /// </summary>
        /// <param name="xRotate">x旋转</param>
        /// <param name="yRotate">y旋转</param>
        public void CameraRotate(float xRotate, float yRotate)
        {
            if (Camera.main == null) return;
            Camera.main.transform.rotation = Camera.main.transform.rotation *
                Quaternion.Euler(yRotate * Time.deltaTime * 100,0, 0);
            gameObject.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, xRotate * Time.deltaTime * 100, 0);
        }

    }
}