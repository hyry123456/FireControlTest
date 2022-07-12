
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float speed = 10;

    /// <summary>    /// 当前速度    /// </summary>
    Vector3 velocal;
    /// <summary>    /// 加速度    /// </summary>
    public float groundAcceleration = 10f, airAcceleration = 5;

    new Rigidbody rigidbody;
    /// <summary>    /// 期望速度    /// </summary>
    Vector3 desiredVelocity;

    private bool desiredJump = false;

    public float jumpHeight = 2f;
    /// <summary>    /// 最大跳跃次数    /// </summary>
    public int maxAirJumps = 2;
    private int airJumps = 0;

    /// <summary>    /// 是否在地面上    /// </summary>
    private bool onGround = false;

    /// <summary>    /// 字段OnSteep，确定是否在斜面上    /// </summary>
    private bool OnSteep => steepContractCount > 0;


    /// <summary>
    /// 最大地面的倾斜夹角，以及楼梯倾斜夹角
    /// </summary>
    [Range(0, 90)]
    public float maxGroundAngle = 25f, maxStairAngle = 25;
    private float minGroundDot = 0, minStairsDot = 0;

    /// <summary>
    /// 接触面的法线，这个法线是平均法线，用来确定移动面的方向以及跳跃的方向
    /// </summary>
    Vector3 contactNormal, steepNormal;
    /// <summary>
    /// 陡峭面检测，如果人物被卡在陡峭面中，需要进行计数，判断是否要陡峭面跳跃
    /// </summary>
    int steepContractCount = 0;

    /// <summary>
    /// 用来确定此时离开地面的时间(stepSinceLastGround)，在地面时会变为0，
    /// 不在时会逐物理帧刷新
    /// </summary>
    int stepSinceLastGround = 0;
    /// <summary>    /// 用来确定跳跃的时间，当跳跃时会归零，在物理帧时逐帧增加    /// </summary>
    int stepSinceLastJump = 0;

    /// <summary>    /// 判断时候可以贴地的速度，如果速度大于该值，不允许贴地    /// </summary>
    [SerializeField, Range(0, 100f)]
    float maxSnapSpeed = 100f;
    /// <summary>    /// 贴地的检测距离    /// </summary>
    [SerializeField, Range(0, 10f)]
    float probeDistance = 3f;

    /// <summary>    /// 贴地检查的层，以及楼梯检查层    /// </summary>
    [SerializeField]
    LayerMask probeMask, stairsMask = -1;

    /// <summary>    /// 输入空间，用来根据该空间来控制模型移动    /// </summary>
    [SerializeField]
    Transform playerInputSpace;

    /// <summary>
    /// 存储三个轴的值，用来确定变化后的重力方向,避免因为移动与重力相抵消导致的bug
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
        //实际上就是加速度是定值，角色输入值是目标速度，逐帧将速度变化为目标速度
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

        //与运算，防止输入被关闭
        desiredJump |= Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        //upAxis = -Physics.gravity.normalized;
        //Vector3 gravity = CustomGravity.GetGravity(rigidbody.position, out upAxis);
        Vector3 gravity = Physics.gravity;

        //更新数据，用来对这一个物理帧的数据进行更新之类的
        UpdateState();
        //确定在空中还是在地面
        AdjustVelocity();

        if (desiredJump)
        {
            Jump(gravity);
            desiredJump = false;
        }
        Debug.DrawLine(transform.position, transform.position + gravity);
        //不再由Unity的刚体运用重力，重力由我们自己添加
        velocal += gravity * Time.deltaTime;

        rigidbody.velocity = velocal;
        ClearState();
    }


    void UpdateState()
    {
        stepSinceLastGround += 1;
        stepSinceLastJump += 1;
        velocal = rigidbody.velocity;
        //当不在地面时执行贴近地面方法
        if (onGround/*在地上*/ || SnapToGround()/*可以贴近地面，也就是刚刚未经过跳跃，但是飞了出去*/ 
            || CheckSteepContacts()/*在斜面上，且被斜面包围*/)
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
        //确定跳跃方向
        if (onGround)
            //在地上，直接根据接触方向
            jumpDirction = contactNormal;
        else if (OnSteep)
        {
            //在斜面就用斜面方向，同时添加一个向上的方向，保证能够往上爬
            jumpDirction = (steepNormal + Vector3.up).normalized;
            airJumps = -1;
        }
        else if (airJumps < maxAirJumps)
        {
            //如果不在地上也不在斜面，并且可以在空中跳跃
            jumpDirction = Vector3.up;
        }
        //不能跳，退出
        else return;

        //根据重力的大小来确定移动速度，因此重力可变了
        float jumpSpeed = Mathf.Sqrt(2f * -gravity.y * jumpHeight);
        //float jumpSpeed = jumpHeight;
        float aligneSpeed = Vector3.Dot(velocal, jumpDirction);
        if (aligneSpeed > 0)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - aligneSpeed, 0);
        }
        velocal += jumpDirction * jumpSpeed;
        airJumps++;

        //跳跃时刷新跳跃时间，保证在前面这段时间不会进行贴地
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
                //保证如果有多个接触面时能够正确的获取法线，因此用加
                contactNormal += normal;
            }
            //陡峭面移动控制，但是避免彻底的垂直面
            else if(upDot > -0.01f)
            {
                steepContractCount++;
                steepNormal += normal;
            }

        }
    }

    //将其中一个轴的值，根据接触面的法线投影到这个接触面上
    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    /// <summary>
    /// 调整移动方向，用来保证移动的方向是沿着平面的
    /// </summary>
    void AdjustVelocity()
    {
        //因为速度用的也是世界坐标，因此移动时投影也依靠的是世界坐标，其中right控制x轴，foward控制Y轴
        //将1，0，0投影到接触平面上，
        //Vector3 xAixs = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 xAixs = ProjectDirectionOnPlane(rightAxis, contactNormal);
        //将0，0，1投影到接触平面上
        //Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;
        Vector3 zAxis = ProjectDirectionOnPlane(forwardAxis, contactNormal);

        //确定实际上在这个平面上的X移动值
        float currentX = Vector3.Dot(velocal, xAixs);
        //确定实际上在这个平面上的Z移动值
        float currentZ = Vector3.Dot(velocal, zAxis);

        float acceleration = onGround ? groundAcceleration : airAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        //确定根据期望得到的移动值
        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        //移动要根据这个平面的方向来移动，因此根据实际值与期望值的差确定要增加的速度大小，
        //然后乘以投影计算出来的X值以及Z值确定最后的移动值
        velocal += xAixs * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    /// <summary>
    /// 清除数据，把一些数据归为初始化
    /// </summary>
    void ClearState()
    {
        onGround = false;
        contactNormal = steepNormal = Vector3.zero;
        steepContractCount = 0;
    }

    /// <summary>
    /// 用于贴近地面用的方法，减少移动时会飞出去的效果
    /// </summary>
    /// <returns>用来配合一些地面检测使用，因此有返回值</returns>
    bool SnapToGround()
    {
        //贴地行为只进行一次，同时用跳跃时间避免跳跃时贴地
        if(stepSinceLastGround > 1 || stepSinceLastJump <= 2)
        {
            return false;
        }
        float speed = velocal.magnitude;
        //大于最大速度，不进行贴地
        if(speed > maxSnapSpeed)
            return false;

        RaycastHit hit;
        if(!Physics.Raycast(rigidbody.position, -upAxis, out hit, probeDistance, probeMask))
            return false;

        //由于重力方向可以改变，因此点乘需要使用改变后的方向
        float upDot = Vector3.Dot(upAxis, hit.normal);
        //如果射中的面不能作为可以站立的面，就不进行贴近
        if(upDot < GetMinDot(hit.collider.gameObject.layer))
            return false;

        contactNormal = hit.normal;

        //确定速度在法线上的大小
        float dot = Vector3.Dot(velocal, hit.normal);
        //保证只有速度朝上时才会往下压，不会减少下落速度
        if (dot > 0)
        {
            //根据速度的大小往平面上压
            velocal = (velocal - hit.normal * dot).normalized * speed;
        }
        return true;
    }

    float GetMinDot(int layer)
    {
        //判断是楼梯还是正常地面
        return (stairsMask & (1 << layer)) == 0 ?
            minGroundDot : minStairsDot;
    }

    /// <summary>
    /// 检查斜面，当被陡峭面围在一起时算是在地上，此时将接触方向就是围绕的法线方向
    /// </summary>
    /// <returns>是否被斜面包围且无法移动</returns>
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

    /// <summary>    /// 确定该方向投影到该平面上的方向值，进行过标准化的    /// </summary>
    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }
}
