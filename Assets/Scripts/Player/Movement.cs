using System.Collections;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
public class Movement : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    public Camera mainCamera;
    [SerializeField] Transform body;
    [SerializeField] TrailRenderer tr;
    float x, z; // wasd ����
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
        //�ٸ� ��ü�� ������ ȸ���� �̵� ����
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (UI.Instance.state == UI.State.Stop)
        {
            return;
        }
        //�⺻���� wasd ����
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(x, 0f, z).normalized * Time.deltaTime * playerSpeed);

        // ���콺�����ʹ�� body, gunPos������ȯ
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
                Debug.Log("�뽬��");
                return; //�뽬���� �Ǵٸ� �뽬 �Ұ�
            }
            CanDash();
            if (canDash)
            {
                DashCount--;
                StartCoroutine(Dash(x, z));
            }
            else if (!canDash)
            {
                Debug.Log("�뽬��Ÿ��");
            }
        }
        //�뽬 �� 2�� ���κ��� ��Ÿ�� �����(���� ��2�ʾ� ����, �ѹ��� �ι��뽬 �����ؾ���)
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
                //Debug.Log("����");
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
        //�߷� ����
        float posY = transform.position.y;
        if (!isJumping)
        {
            // ���������� �ƴѵ� ���߿� ������ ����
            if (!IsGround)
            {
                transform.Translate(Vector3.down * gravity * Time.deltaTime);
                // �������� ����ɶ����� gravity ����(����)
                gravity += weight;
            }
            //���������� �ƴҶ� �ٴڿ� ������
            else if (IsGround)
            {
                gravity = 0.4f; //���ӵ� �߷� �ʱ�ȭ
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
        //�����ȵɶ��� �����̸鼭 ��������Ƚ���� 0�϶���
        //���ٴ��̰ų� �����̴��� ��������Ƚ��1�̻��̸� ���� ��������
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