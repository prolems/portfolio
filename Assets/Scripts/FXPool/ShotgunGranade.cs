using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunGranade : MonoBehaviour,IDamageable
{
    private ShotGun sg;
    [SerializeField] ParticleSystem fxExplosion;
    [SerializeField] private AudioClip clip;
    float hp = 10;
    float speed = 20f;
    float gravity = 0.001f;
    float weight = 0.005f;
    float timeLimit = 2f;
    float curtime = 0f;
    float maxRadius = 4.5f;
    float deffaultRadius = 0.5f;
    bool isBombing = false;
    bool isSticked = false;
    SphereCollider coll;

    
    public void Damage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0 && !isSticked)
        {
            StartCoroutine(InstantExplosion());
        }
    }
    public void SetShotgun(ShotGun sg)
    {
        this.sg = sg;
    }
  
  
    private void OnEnable()
    {
        isBombing = false;
        isSticked = false;
    }
    void Start()
    {
        coll = transform.GetComponent<SphereCollider>();
    }
    private void Update()
    {
        if (!isSticked)
        {

            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.Translate(Vector3.down * Time.deltaTime * gravity);
            // �������� ����ɶ����� gravity ����(����)
            gravity += weight;
            // ��𿡵� �Ⱥپ����� ���߿��� 2�ʵ� ����
            curtime += Time.deltaTime;
            if (curtime > timeLimit)
            {
                curtime = 0;
                StartCoroutine(InstantExplosion());
            }
        }
        else if (isSticked)
        {
         
            gravity = 0.05f; //���ӵ� �߷� �ʱ�ȭ
            //transform.position = Vector3.Lerp(transform.position, newPos, 0.2f);
        }
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1);
        SoundManager.instance.SFXPlay("explosion", clip);
        isBombing = true;
        coll.radius = maxRadius;
        coll.isTrigger = true;
        fxExplosion.Play();
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        End();
    }
    IEnumerator InstantExplosion()
    {
        SoundManager.instance.SFXPlay("explosion", clip);
        isBombing = true;
        coll.radius = maxRadius;
        coll.isTrigger = true;
        fxExplosion.Play();
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        End();
    }


    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.rigidbody != null)
        {
            if(gameObject.GetComponent<FixedJoint>() == null)
            {
                var joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = collision.rigidbody;
                joint.enableCollision = true;
            }
        }
        isSticked = true;
        StartCoroutine(Explosion());
    }
    public void OnTriggerEnter(Collider other)
    {
       
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (other.name =="Player" )
            {
                //Player �ڽ��� �¾����� �� �������� �±�
                if(isBombing )
                {
                    damageable?.Damage(30);
                    Debug.Log("�ڱⵥ����30");
                }
            }
            else if(other.name == "IsGroundCheck")
            {
                damageable?.Damage(0);
            }
            else
            {
                Debug.Log(isSticked);
                if (isBombing)
                {
                    if (other.gameObject.CompareTag("Boss"))
                    {
                    //���� ������ �ǰ� or �ѹ��� ���߱� ������ 
                        damageable?.Damage(sg.Damage_1);
                        isBombing = false;
                    }
                    else
                    {
                        damageable?.Damage(sg.Damage_1);
                    }
                }
            }
    }
    public void End()
    {
        curtime = 0;
        transform.SetParent(null);
        isBombing = false;
        coll.isTrigger = false;
        coll.radius = deffaultRadius;
        isSticked = false;
        GetComponent<MeshRenderer>().enabled = true;
        Destroy(gameObject.GetComponent<FixedJoint>());
        FX_Pool.instance.SGGranadeEnd(this);
    }
}
