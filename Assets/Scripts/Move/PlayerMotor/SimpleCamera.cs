
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    /// <summary>
    /// ���������������ľ���
    /// </summary>
    [SerializeField, Range(1f, 20f)]
    float distance = 5f;

    /// <summary>
    /// ������ƫ�ƾ��룬��Ŀ�����ʵ����������ڸþ���ʱ���Ż�ӽ�������
    /// </summary>
    [SerializeField, Min(0f)]
    float focusRadius = 1f;

    /// <summary>    /// Ҫ������Ŀ��㣬��������ƶ����õ���,
    /// �����������ʵ��Ŀ������λ�ã����ܻ���һ����ƫ��    /// </summary>
    Vector3 focusPoint;
    /// <summary>   /// ��һ��������Ŀ��㣬�����ж��ƶ���������ȷ���ӽǽӽ�������    /// </summary>
    Vector3 previousFocusPoint;
    /// <summary>    /// ÿ��Ľӽ���������Ϊ0.5����ÿ����Сһ��    /// </summary>
    [SerializeField,Range(0f, 1f)]
    float focusCentering = 0.5f;

    /// <summary>    /// �洢��ǰ��ת�Ƕ�    /// </summary>
    Vector2 oribitAngles = new Vector2(0f, 0f);

    /// <summary>    /// ��ֱ��ת�Ƕ�����    /// </summary>
    [SerializeField, Range(-89f, 89f)]
    float minVerticalAngle = -30f, maxVerticalAngle = 60;

    /// <summary>    /// ����Ҿ����������õ�ʱ��δ����󣬽������������    /// </summary>
    [SerializeField]
    float alignDelay = 5;
    /// <summary>    /// ����������ƶ�ʱ��С�ڸýǶ�ʱ����л����ƶ���������ÿʱÿ�̶���������ƶ�������ٶ��ƶ�    /// </summary>
    [SerializeField, Range(0, 90)]
    float alignSmoothRange = 45f;

    /// <summary>0    /// ��һ��������ת��ʱ��    /// </summary>
    float lastManualRotationTime = 0;

    [SerializeField, Range(1f, 360f)]
    float rotationSpeed = 90f;

    /// <summary>    /// ���ò��ܱ�����������Ĳ�    /// </summary>
    [SerializeField]
    LayerMask layerMask = -1;

    /// <summary>    /// ��������    /// </summary>
    Camera regularCamera;

    float extendsTan = 0;

    /// <summary>    /// ��������ת����ת�Ƕ�    /// </summary>
    //Quaternion gravityAligment = Quaternion.identity;

    /// <summary>    /// Ϊ�˱�֤Ŀǰ��ת�ĽǶȲ����������仯Ӱ�죬���ʹ������洢��תֵ    /// </summary>
    Quaternion orbitRotation;

    //[SerializeField, Min(0f)]
    //float upAlignmentSpeed = 360f;

    /// <summary>    /// ȷ���������ͶӰ����Ĵ�С    /// </summary>
    Vector3 CameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            //ȷ��ͶӰ���ε�Y��һ���С����ֱ��ʹ�ý�ƽ���һ������ΪfieldOfView�����һ�������ţ�������Ҫ�����ű��ȥ
            halfExtends.y = regularCamera.nearClipPlane * extendsTan;
            //���ݱ��������X���С
            halfExtends.x = halfExtends.y * regularCamera.aspect;
            //Z����ͶӰ���������ã�û��Ӱ��
            halfExtends.z = 0f;
            return halfExtends;
        }
    }



    void Start()
    {
        //��ͷ�ȿ���Ŀ��λ��
        focusPoint = focus.position;
        regularCamera = GetComponent<Camera>();
        extendsTan = Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
        transform.localRotation = orbitRotation = Quaternion.Euler(oribitAngles);
    }

    private void LateUpdate()
    {
        //UpdateGravityAlignemt();
        //gravityAligment = Quaternion.FromToRotation(
        //    gravityAligment * Vector3.up, -Physics.gravity.normalized
        //    ) * gravityAligment;

        UpdateFocusPoint();

        if (ManualRotation() || AutomaticRotation())
        {
            //ȷ����ת�Ƕ�
            ConstrainAngle();
            //ֻ����Ҫ��תʱ�Ż��Ŀǰ���������ת���������ת
            orbitRotation = Quaternion.Euler(oribitAngles);
        }

        //Quaternion lookRotation = gravityAligment * orbitRotation;

        //������ռ��ǰ����X�Լ�ZֵͶӰ��Ŀǰ��ѡ��Ƕ���
        //Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookDirection = orbitRotation * Vector3.forward;

        //������λ�ò���ʵ�ʼ�����λ�ã�������Ϊ�������ǵĽ�������ڼ������ڲ�(��Ϊ��ƫ��)��
        //�����������ķ���һ������ȷ�ģ���Ҫ����һ����ƫ��
        Vector3 lookPosition = focusPoint - lookDirection * distance;

        Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
        //ȷ������ƫ�ƺ�������λ��
        Vector3 rectPosition = lookPosition + rectOffset;
        Vector3 castFrom = focus.position;
        //ȷ��ƫ�Ƶ������λ�þ���ʵ��������λ�õķ���
        Vector3 castLine = rectPosition - castFrom;
        //ȷ������
        float castDistance = castLine.magnitude;
        //��׼������
        Vector3 castDirection = castLine / castDistance;

        //����ͶӰ���Ͼ��������һ�����Σ�ͶӰ�����Ǵ������㵽Ŀ��㣬����������н�ƽ�棬
        //��˲��ÿ�����Ҫ�����з���ǰ��֮�������
        //if(Physics.BoxCast(castFrom, CameraHalfExtends, castDirection,out RaycastHit hit, lookRotation,
        //    castDistance, layerMask))
        if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit, orbitRotation,
            castDistance, layerMask))
        {
            //ȷ��ʵ����ײλ�ã�Ϊ�˱��⽹�������ģ��������ͶӰ����ʵ�����Ǵ�����λ��ͶӰ�������λ�ã�
            //�ж���û����ײ�㣬Ȼ��������
            rectPosition = castFrom + castDirection * hit.distance;
            //��Ϊ��ƫ�Ƽ��㣬�����Ҫ��ƫ��ֵת������
            lookPosition = rectPosition - rectOffset;
        }

        //�������������������Լ����������λ����
        //transform.SetPositionAndRotation(lookPosition, lookRotation);
        transform.SetPositionAndRotation(lookPosition, orbitRotation);
    }


    /// <summary>    /// ȷ����Ҫ��ת���ķ���    /// </summary>
    void UpdateGravityAlignemt()
    {
        //Vector3 fromUp = gravityAligment * Vector3.up;
        //Vector3 toUp = CustomGravity.GetUpAxis(focusPoint);


        //float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp), -1f, 1f);
        //float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //float maxAngle = upAlignmentSpeed * Time.deltaTime;

        //Quaternion newAlignment = Quaternion.FromToRotation(fromUp, toUp) * gravityAligment;

        //if (angle <= maxAngle)
        //{
        //    gravityAligment = newAlignment;
        //}
        //else
        //{
        //    gravityAligment = Quaternion.Slerp(
        //        gravityAligment, newAlignment, maxAngle / angle);
        //}
    }

    /// <summary>
    /// ������ת�Ƕȣ�ͬʱ�����ж�����Ƿ������룬������ͷ���false
    /// </summary>
    /// <returns></returns>
    bool ManualRotation()
    {
        //�������ֵ
        Vector2 input = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        float e = 0.001f;
        //�ж��Ƿ�������,ע�����������
        if(input.x < -e || input.x > e || input.y  < -e || input.y > e)
        {
            //��������ֵ���ƽǶ�
            oribitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            //��������ʱ��
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }
        return false;
    }

    /// <summary>
    /// ����Ŀ��۲��
    /// </summary>
    void UpdateFocusPoint()
    {
        previousFocusPoint = focusPoint;
        //����Ӧ�ÿ���ĵ�
        Vector3 targetPoint = focus.position;
        //ֻ����Ҫ��Χ�۽��Ż��ƶ��ӳ��ƶ������
        if(focusRadius > 0f)
        {
            //ȷ��ʵ�ʿ����������뿴���ľ���
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            //�õڶ�����ֵ�����ڲ���������Ҳ����˵���м�ʱҲ��ӽ������㣬�������м�ʱ��С�ıȽ���
            if(focusCentering > 0f)
            {
                //ȷ�����ڲ��Ľ����ٶȣ�����ÿ��Ľӽ�����
                t = 1f - focusCentering * Time.unscaledDeltaTime;
            }
            //ֻ�д������ƫ�ƾ���ʱ�Ż��ƶ�������
            if(distance > focusRadius)
            {
                //ȷ�������С��ȡ������ֵ����Сֵ
                t = Mathf.Min(t, focusRadius / distance);
            }
            //��Ҫע����ǣ�����ķ�ʽ��begin����Ŀ��λ�ã����ֻ�в��Խ��ֵԽС��Խ�ӽ�Ŀ���ٶ�
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
            focusPoint = targetPoint;
    }

    /// <summary>    /// �ڱ༭���е����Ʒ���    /// </summary>
    private void OnValidate()
    {
        if(maxVerticalAngle < minVerticalAngle)
            maxVerticalAngle = minVerticalAngle;
    }

    /// <summary>    /// ���ƽǶȴ�С    /// </summary>
    void ConstrainAngle()
    {
        oribitAngles.x = Mathf.Clamp(oribitAngles.x, minVerticalAngle, maxVerticalAngle);
        if(oribitAngles.y < 0f)
        {
            oribitAngles.y += 360f;
        }
        else if(oribitAngles.y >= 360f)
        {
            oribitAngles.y -= 360f;
        }
    }

    /// <summary>
    /// �Զ���ת����������ʵ�ִ󲿷���Ϸ���ƶ�ʱ��һ��ʱ��û�����룬
    /// ͬʱ�����ڼ�������ƶ�ʱ��������������������ģ�͵ĺ󲿵�Ч��
    /// </summary>
    /// <returns>�Ƿ�Ҫ������ת</returns>
    bool AutomaticRotation()
    {
        //�ж��Ƿ񵽴�ת�ӽǵ�ʱ��
        if (Time.unscaledTime - lastManualRotationTime < alignDelay)
            return false;

        //��֮ǰ���ƶ�ֵͨ����ת��ת�ǶȽ��и�ԭ��ȷ�����ڵ�x��z�ƶ�ֵ��ͨ�����ֵ������ת
        //Vector3 alignedDelta = Quaternion.Inverse(gravityAligment) * (focusPoint - previousFocusPoint);
        //�ж��ƶ�����
        //Vector2 movement = new Vector2(alignedDelta.x - alignedDelta.x,
        //    focusPoint.z - previousFocusPoint.z);
        Vector2 movement = new Vector2(focusPoint.x - previousFocusPoint.x,
            focusPoint.z - previousFocusPoint.z);

        //ȷ���ƶ�����
        float movementDeltaSqr = movement.sqrMagnitude;
        //����ƶ���̫С���Ͳ���ת
        if (movementDeltaSqr < 0.0000001f)
            return false;
        //ȷ��Ҫ�仯���ĽǶ�
        float headingAngle = GetAngle(movement.normalized);
        //�õ���ǰ��ת�Ƕ���Ŀ����ת�Ƕȵ���С���������ҷ��ؾ���ֵ
        float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(oribitAngles.y, headingAngle));
        //�����ƶ��ٶȣ�����ܴ���֡��࣬���ǿ����ƶ�����̫С����˸ı��ٶ�ҲС
        float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
        //������ת�ٶȣ�����Ŀ����ת�ǶȱȽϽӽ�ʱ����С��ת�ٶ�
        if (deltaAbs < alignSmoothRange)
        {
            rotationChange *= deltaAbs / alignSmoothRange;
        }
        //���Ƿ����෴�����Ǽн�ҲС�����
        else if(180f - deltaAbs < alignSmoothRange)
        {
            rotationChange *= (180f - deltaAbs) / alignSmoothRange;
        }
        //����ģʽ
        oribitAngles.y = Mathf.MoveTowardsAngle(oribitAngles.y, headingAngle, rotationChange);

        return true;
    }

    /// <summary>
    /// �����ƶ��Ĳ��ֵ�ж���ת�Ƕȣ�ע�⴫��ֵҪ��׼��
    /// </summary>
    static float GetAngle(Vector2 direction)
    {
        //ͨ�������Һ����������ת������ƶ���������Ҫ��yֵ�Ƕ�
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        //�ж����ıߣ�Ҳ����˳ʱ�뻹����ʱ��
        return direction.x < 0f ? 360f - angle : angle;
    }
}
