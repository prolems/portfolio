using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable
{
    [SerializeField] ParticleSystem fxBlood;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Transform p;
    [SerializeField] Stats stats;
    [SerializeField] Transform attPos;
    [SerializeField] RifleMag rifleMag;
    [SerializeField] ShotgunMag shotgunMag;
    public float HP { get; set; }
    public float Speed { get; set; }
    float coolTime = 1.5f;
    float nextAtt;
    public float AttDistance { get; set; }
    bool dead = false;
    public void Damage(float dmg)
    {
        if (HP > 0)
        {
            HP -= dmg;
            if (HP <= 0)
            {
                HP = 0;
                Dead();
            }
        }
    }

    void Start()
    {
        HP = 180;
        Speed = 1.5f;
        AttDistance = 2;
    }
    void Dead()
    {
        dead = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        fxBlood.Play();
        Destroy(gameObject,fxBlood.main.duration);
        SpawnItem();
    }
   void SpawnItem()
    {
        int random = Random.Range(0, 100);
        if(random< 30)
        {
            GameObject item = Instantiate(rifleMag.gameObject, transform.position, Quaternion.identity);
            item.GetComponent<RifleMag>().SetStats(stats);
        }
        else if(random < 60)
        {
            GameObject item = Instantiate(shotgunMag.gameObject, transform.position, Quaternion.identity);
            item.GetComponent<ShotgunMag>().SetStats(stats);
        }
        else
        {
            return;
        }
    }
    void Update()
    {
        if (UI.Instance.state == UI.State.Stop || dead)
            return;

        //다른 물체랑 비볐을때 회전밑 이동 금지
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 dir = p.transform.position - transform.position;
        dir.Normalize();
        float dis = Vector3.Distance(p.transform.position, transform.position);
        if (dis <= 1.5f)
        {
            // attack method
            //Debug.Log(dis);
            if (Time.time > nextAtt)
            {
                nextAtt = Time.time + coolTime;
                Attack(dir);
            }
        }
        else if (dis <= 3.5f)
        {
            Move(dir, Speed * 3);

        }
        else if (dis < 7.5f)
        {
            Move(dir, Speed);
        }
    }
    void Move(Vector3 dir, float speed)
    {
        float rot = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, (rot - 90) * -1, 0);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    void Attack(Vector3 dir)
    {
        attPos.GetComponent<LineRenderer>().SetPosition(0,attPos.position);
        //플레이어 사이에 맵이 껴있으면 인식하게끔,자기자신이 데미지 입는 문제 해결
        int layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Map");
        if(Physics.Raycast(attPos.position, dir, out RaycastHit hit, AttDistance, layerMask))
        {
            Debug.Log(hit.collider.name);
            attPos.GetComponent<LineRenderer>().SetPosition(1,hit.point);
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(40);
            }
        }
        else
        {
            attPos.GetComponent<LineRenderer>().SetPosition(1,attPos.position+dir*AttDistance);
        }
        StartCoroutine(LineDuration());
    }
    IEnumerator LineDuration()
    {
        attPos.GetComponent<LineRenderer>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        attPos.GetComponent<LineRenderer>().enabled = false;
    }
}
