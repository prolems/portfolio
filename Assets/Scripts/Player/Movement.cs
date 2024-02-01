using System.Collections;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
public class Movement : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    public Camera mainCamera;
    [SerializeField] Transform body;
    [SerializeField] TrailRenderer tr;
    float x, z; // wasd 무빙
    public float playerSpeed;

    public float dashSpeed, dashTime, jumpTime;

    bool isGround = true;
    public bool IsGround { get { return isGround; } set { isGround = value; } }
    bool isDashing = false;
    bool canDash = true;
    public int MaxDashCount { get; set; }
    public int DashCount { get; set; }
    public float DashCoolTime { get; set; }
    float curDashTime;

    public int JumpCount { get; set; }

    Vector3 dir = Vector3.zero;
    string[] soundType = new string[] { "dash", "jump", };

    void Sound(string soundType)
    {
        if (soundType == "dash")
        {
            SoundManager.instance.SFXPlay("dash", clips[0]);
        }
        else if (soundType == "jump")
        {
            SoundManager.instance.SFXPlay("jump", clips[1]);
        }
    }

    void Start()
    {
        IsGround = false;
        mainCamera = Camera.main;
        dashSpeed = 20;
        DashCoolTime = 2;
        playerSpeed = 6f;
        MaxDashCount = 2;
        DashCount = MaxDashCount;
        JumpCount = 1;
    }
    void Update()
    {
        //다른 물체랑 비볐을때 회전밑 이동 금지
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (UI.Instance.state == UI.State.Stop)
        {
            return;
        }
        //기본적인 wasd 조작
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(x, 0f, z).normalized * Time.deltaTime * playerSpeed);

        // 마우스포인터대로 body, gunPos방향전환
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePosition = hit.point;
            dir = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            body.eulerAngles = new Vector3(0, (angle - 90) * -1, 0);
        }



        if (Input.GetKeyDown(KeyCode.LeftShift) && (x != 0 || z != 0))
        {
            if (isDashing)
            {
                Debug.Log("대쉬중");
                return; //대쉬도중 또다른 대쉬 불가
            }
            CanDash();
            if (canDash)
            {
                DashCount--;
                StartCoroutine(Dash(x, z));
            }
            else if (!canDash)
            {
                Debug.Log("대쉬쿨타임");
            }
        }
        //대쉬 각 2개 개인별로 쿨타임 만들기(각자 쿨2초씩 적용, 한번에 두번대쉬 가능해야함)
        if (DashCount < MaxDashCount)
        {
            curDashTime += Time.deltaTime;
            if (curDashTime > DashCoolTime)
            {
                curDashTime = 0;
                DashCount++;
                if (DashCount > MaxDashCount)
                {
                    DashCount = MaxDashCount;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CanJump();
            if (canJump)
            {
                //Debug.Log("점프");
                FX_Pool.instance.FxJump(transform.position);
                StartCoroutine(Jump());
                if (!IsGround)
                {
                    JumpCount--;
                    if (JumpCount <= 0)
                    {
                        JumpCount = 0;
                    }
                }
            }
        }
        //중력 구현
        float posY = transform.position.y;
        if (!isJumping)
        {
            // 점프도중이 아닌데 공중에 있을때 낙하
            if (!IsGround)
            {
                transform.Translate(Vector3.down * gravity * Time.deltaTime);
                // 매프레임 실행될때마다 gravity 증가(가속)
                gravity += weight;
            }
            //점프도중이 아닐때 바닥에 있을때
            else if (IsGround)
            {
                gravity = 0.4f; //가속된 중력 초기화
            }
        }

    }
    bool CanDash()
    {
        if (DashCount <= 0)
        {
            DashCount = 0;
            canDash = false;
        }
        else
        {
            canDash = true;
        }
        return canDash;
    }
    private IEnumerator Dash(float x, float z)
    {
        Sound(soundType[0]);
        isDashing = true;
        tr.emitting = true;
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            transform.Translate(new Vector3(x, 0f, z).normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }
        isDashing = false;
        tr.emitting = false;
    }

    public bool isJumping = false;
    bool canJump = true;
    float jumpSpeed = 10f;
    float gravity = 0.1f;
    float weight = 0.05f;
    private IEnumerator Jump()
    {
        Sound(soundType[1]);
        isJumping = true;
        float startTime = Time.time;
        while (Time.time < startTime + jumpTime)
        {
            transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);

            yield return null;
        }
        isJumping = false;
        yield return null;
    }
    bool CanJump()
    {
        //점프안될때는 공중이면서 점프가능횟수가 0일때만
        //땅바닥이거나 공중이더라도 점프가능횟수1이상이면 전부 점프가능
        if (!IsGround && JumpCount <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
        return canJump;
    }
}