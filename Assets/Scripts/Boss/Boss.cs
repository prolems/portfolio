using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform p;
    [SerializeField] ParticleSystem fxBlood;
    public float maxHp = 12000;
    private float hp = 12000;
    public float HP
    {
        get { return hp; }
        set { hp = value;}
    }

    bool isSee;
    bool attackCool = false;
    bool isPattern = false;
    float walkTime = 3f;
    float rushTime = 0.7f;
    public void Dead()
    {
        fxBlood.Play();
        StopAllCoroutines();
        isPattern = false;
        isSee = false;
        Destroy(gameObject,1f);
        UI.Instance.state = UI.State.Stop;
        UI.Instance.completePanel.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.name == "Player")
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null && !attackCool )
            {
                damageable.Damage(40);
                attackCool = true;
                Invoke("Cooltime", 1.5f);
            }
        }
    }
    void Cooltime()
    {
        attackCool = false;
    }
    void Start()
    {
        HP = 12000;
        isSee = true;
    }
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (UI.Instance.state == UI.State.Stop)
        {
            StopAllCoroutines();
            isPattern = false;
            isSee =false;
            return;
        }
        if(HP > 0)
        {
            Vector3 dir = p.transform.position - transform.position;
            float dis = Vector3.Distance(p.transform.position, transform.position);
            if (dis <= 12)
            {
                if (!isPattern)
                {
                    isSee = true;
                    StartCoroutine(Pattern(dir.normalized));
                }
            }
            else if (dis > 12)
            {
                StopAllCoroutines();
                isPattern = false;
                isSee = false;
            }
            See(dir);
        }
    }
    IEnumerator Pattern(Vector3 dir)
    {
        // 전체적인 보스 패턴
        //좀 걷다가(보면서)-> 멈췄다가(보면서)->
        //돌진(한방향)-> 멈췄다가-> 다시 걷기(보면서)로 반복
        isPattern = true;
        yield return Walk();
        yield return Rush();
        isPattern = false;
    }
    void See(Vector3 dir)
    {
        if(isSee) 
        {
            float rot = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, (rot - 90) * -1, 0);
            //부드럽게 회전 slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, (rot - 90) * -1, 0), 6*Time.deltaTime);
        }
    }
    IEnumerator Walk()
    {
        float curTime = Time.time;
        while(Time.time<curTime+walkTime)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 1);
            yield return null;
        }
    }
    IEnumerator Rush()
    {
        //돌진 패턴(방향은 일직선,See함수 작동x)
        yield return new WaitForSeconds(0.6f);
        isSee = false;
        float curTime = Time.time;
        while (Time.time<curTime+rushTime) 
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 15);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        isSee = true;
    }
}
