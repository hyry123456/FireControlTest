
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float speed = 10;

    /// <summary>    /// ��ǰ�ٶ�    /// </summary>
    Vector3 velocal;
    /// <summary>    /// ���ٶ�    /// </summary>
    public float groundAcceleration = 10f, airAcceleration = 5;

    new Rigidbody rigidbody;
    /// <summary>    /// �����ٶ�    /// </summary>
    Vector3 desiredVelocity;

    private bool desiredJump = false;

    public float jumpHeight = 2f;
    /// <summary>    /// �����Ծ����    /// </summary>
    public int maxAirJumps = 2;
    private int airJumps = 0;

    /// <summary>    /// �Ƿ��ڵ�����    /// </summary>
    private bool onGround = false;

    /// <summary>    /// �ֶ�OnSteep��ȷ���Ƿ���б����    /// </summary>
    private bool OnSteep => steepContractCount > 0;


    /// <summary>
    /// ���������б�нǣ��Լ�¥����б�н�
    /// </summary>
    [Range(0, 90)]
    public float maxGroundAngle = 25f, maxStairAngle = 25;
    private float minGroundDot = 0, minStairsDot = 0;

    /// <summary>
    /// �Ӵ���ķ��ߣ����������ƽ�����ߣ�����ȷ���ƶ���ķ����Լ���Ծ�ķ���
    /// </summary>
    Vector3 contactNormal, steepNormal;
    /// <summary>
    /// �������⣬������ﱻ���ڶ������У���Ҫ���м������ж��Ƿ�Ҫ��������Ծ
    /// </summary>
    int steepContractCount = 0;

    /// <summary>
    /// ����ȷ����ʱ�뿪�����ʱ��(stepSinceLastGround)���ڵ���ʱ���Ϊ0��
    /// ����ʱ��������֡ˢ��
    /// </summary>
    int stepSinceLastGround = 0;
    /// <summary>    /// ����ȷ����Ծ��ʱ�䣬����Ծʱ����㣬������֡ʱ��֡����    /// </summary>
    int stepSinceLastJump = 0;

    /// <summary>    /// �ж�ʱ��������ص��ٶȣ�����ٶȴ��ڸ�ֵ������������    /// </summary>
    [SerializeField, Range(0, 100f)]
    float maxSnapSpeed = 100f;
    /// <summary>    /// ���صļ�����    /// </summary>
    [SerializeField, Range(0, 10f)]
    float probeDistance = 3f;

    /// <summary>    /// ���ؼ��Ĳ㣬�Լ�¥�ݼ���    /// </summary>
    [SerializeField]
    LayerMask probeMask, stairsMask = -1;

    /// <summary>    /// ����ռ䣬�������ݸÿռ�������ģ���ƶ�    /// </summary>
    [SerializeField]
    Transform playerInputSpace;

    /// <summary>
    /// �洢�������ֵ������ȷ���仯�����������,������Ϊ�ƶ���������������µ�bug
    /// </summary>
    Vector3 upAxis, rightAxis, forwardAxis;

    void Start()
    {
        velocal = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        minGroundDot = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDot = Mathf.Cos(maxStairAngle * Mathf.Deg2Rad);
        rigidbody.useGravity = false;
    }

    void Update()
    {
        //ʵ���Ͼ��Ǽ��ٶ��Ƕ�ֵ����ɫ����ֵ��Ŀ���ٶȣ���֡���ٶȱ仯ΪĿ���ٶ�
        Vector2 playInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playInput = Vector2.ClampMagnitude(playInput, 1);

        if (playerInputSpace)
        {
            //rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
            rightAxis = playerInputSpace.right;
            //forwardAxis = ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
            forwardAxis = playerInputSpace.forward;
        }
        else
        {
            rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);

            forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
        }
        desiredVelocity = new Vector3(playInput.x, 0f, playInput.y) * speed;

        //�����㣬��ֹ���뱻�ر�
        desiredJump |= Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        //upAxis = -Physics.gravity.normalized;
        //Vector3 gravity = CustomGravity.GetGravity(rigidbody.position, out upAxis);
        Vector3 gravity = Physics.gravity;

        //�������ݣ���������һ������֡�����ݽ��и���֮���
        UpdateState();
        //ȷ���ڿ��л����ڵ���
        AdjustVelocity();

        if (desiredJump)
        {
            Jump(gravity);
            desiredJump = false;
        }
        Debug.DrawLine(transform.position, transform.position + gravity);
        //������Unity�ĸ������������������������Լ����
        velocal += gravity * Time.deltaTime;

        rigidbody.velocity = velocal;
        ClearState();
    }


    void UpdateState()
    {
        stepSinceLastGround += 1;
        stepSinceLastJump += 1;
        velocal = rigidbody.velocity;
        //�����ڵ���ʱִ���������淽��
        if (onGround/*�ڵ���*/ || SnapToGround()/*�����������棬Ҳ���Ǹո�δ������Ծ�����Ƿ��˳�ȥ*/ 
            || CheckSteepContacts()/*��б���ϣ��ұ�б���Χ*/)
        {
            stepSinceLastGround = 0;
                airJumps = 0;

            contactNormal.Normalize();
        }
        else
            contactNormal = upAxis;
    }

    void Jump(Vector3 gravity)
    {
        Vector3 jumpDirction;
        //ȷ����Ծ����
        if (onGround)
            //�ڵ��ϣ�ֱ�Ӹ��ݽӴ�����
            jumpDirction = contactNormal;
        else if (OnSteep)
        {
            //��б�����б�淽��ͬʱ���һ�����ϵķ��򣬱�֤�ܹ�������
            jumpDirction = (steepNormal + Vector3.up).normalized;
            airJumps = -1;
        }
        else if (airJumps < maxAirJumps)
        {
            //������ڵ���Ҳ����б�棬���ҿ����ڿ�����Ծ
            jumpDirction = Vector3.up;
        }
        //���������˳�
        else return;

        //���������Ĵ�С��ȷ���ƶ��ٶȣ���������ɱ���
        float jumpSpeed = Mathf.Sqrt(2f * -gravity.y * jumpHeight);
        //float jumpSpeed = jumpHeight;
        float aligneSpeed = Vector3.Dot(velocal, jumpDirction);
        if (aligneSpeed > 0)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - aligneSpeed, 0);
        }
        velocal += jumpDirction * jumpSpeed;
        airJumps++;

        //��Ծʱˢ����Ծʱ�䣬��֤��ǰ�����ʱ�䲻���������
        stepSinceLastJump = 0;
    }

    private void OnCollisionExit(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        float minDot = GetMinDot(collision.gameObject.layer);
        for(int i=0; i<collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if(upDot >= minDot)
            {
                onGround = true;
                //��֤����ж���Ӵ���ʱ�ܹ���ȷ�Ļ�ȡ���ߣ�����ü�
                contactNormal += normal;
            }
            //�������ƶ����ƣ����Ǳ��⳹�׵Ĵ�ֱ��
            else if(upDot > -0.01f)
            {
                steepContractCount++;
                steepNormal += normal;
            }

        }
    }

    //������һ�����ֵ�����ݽӴ���ķ���ͶӰ������Ӵ�����
    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    /// <summary>
    /// �����ƶ�����������֤�ƶ��ķ���������ƽ���
    /// </summary>
    void AdjustVelocity()
    {
        //��Ϊ�ٶ��õ�Ҳ���������꣬����ƶ�ʱͶӰҲ���������������꣬����right����x�ᣬfoward����Y��
        //��1��0��0ͶӰ���Ӵ�ƽ���ϣ�
        //Vector3 xAixs = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 xAixs = ProjectDirectionOnPlane(rightAxis, contactNormal);
        //��0��0��1ͶӰ���Ӵ�ƽ����
        //Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;
        Vector3 zAxis = ProjectDirectionOnPlane(forwardAxis, contactNormal);

        //ȷ��ʵ���������ƽ���ϵ�X�ƶ�ֵ
        float currentX = Vector3.Dot(velocal, xAixs);
        //ȷ��ʵ���������ƽ���ϵ�Z�ƶ�ֵ
        float currentZ = Vector3.Dot(velocal, zAxis);

        float acceleration = onGround ? groundAcceleration : airAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        //ȷ�����������õ����ƶ�ֵ
        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        //�ƶ�Ҫ�������ƽ��ķ������ƶ�����˸���ʵ��ֵ������ֵ�Ĳ�ȷ��Ҫ���ӵ��ٶȴ�С��
        //Ȼ�����ͶӰ���������Xֵ�Լ�Zֵȷ�������ƶ�ֵ
        velocal += xAixs * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    /// <summary>
    /// ������ݣ���һЩ���ݹ�Ϊ��ʼ��
    /// </summary>
    void ClearState()
    {
        onGround = false;
        contactNormal = steepNormal = Vector3.zero;
        steepContractCount = 0;
    }

    /// <summary>
    /// �������������õķ����������ƶ�ʱ��ɳ�ȥ��Ч��
    /// </summary>
    /// <returns>�������һЩ������ʹ�ã�����з���ֵ</returns>
    bool SnapToGround()
    {
        //������Ϊֻ����һ�Σ�ͬʱ����Ծʱ�������Ծʱ����
        if(stepSinceLastGround > 1 || stepSinceLastJump <= 2)
        {
            return false;
        }
        float speed = velocal.magnitude;
        //��������ٶȣ�����������
        if(speed > maxSnapSpeed)
            return false;

        RaycastHit hit;
        if(!Physics.Raycast(rigidbody.position, -upAxis, out hit, probeDistance, probeMask))
            return false;

        //��������������Ըı䣬��˵����Ҫʹ�øı��ķ���
        float upDot = Vector3.Dot(upAxis, hit.normal);
        //������е��治����Ϊ����վ�����棬�Ͳ���������
        if(upDot < GetMinDot(hit.collider.gameObject.layer))
            return false;

        contactNormal = hit.normal;

        //ȷ���ٶ��ڷ����ϵĴ�С
        float dot = Vector3.Dot(velocal, hit.normal);
        //��ֻ֤���ٶȳ���ʱ�Ż�����ѹ��������������ٶ�
        if (dot > 0)
        {
            //�����ٶȵĴ�С��ƽ����ѹ
            velocal = (velocal - hit.normal * dot).normalized * speed;
        }
        return true;
    }

    float GetMinDot(int layer)
    {
        //�ж���¥�ݻ�����������
        return (stairsMask & (1 << layer)) == 0 ?
            minGroundDot : minStairsDot;
    }

    /// <summary>
    /// ���б�棬����������Χ��һ��ʱ�����ڵ��ϣ���ʱ���Ӵ��������Χ�Ƶķ��߷���
    /// </summary>
    /// <returns>�Ƿ�б���Χ���޷��ƶ�</returns>
    bool CheckSteepContacts()
    {
        if (steepContractCount > 1)
        {
            steepNormal.Normalize();
            contactNormal = steepNormal;
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDot)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>    /// ȷ���÷���ͶӰ����ƽ���ϵķ���ֵ�����й���׼����    /// </summary>
    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }
}
