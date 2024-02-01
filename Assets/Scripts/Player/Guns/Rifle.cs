using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun, IShootable
{
    [SerializeField] private AudioClip[] rifleClips;
    string[] soundType = new string[] { "fire0", "fire1", "reload" };

    void Sound(string soundType)
    {
        if (soundType == "fire0")
        {
            SoundManager.instance.SFXPlay("rifle0", rifleClips[0]);
        }
        else if (soundType == "fire1")
        {
            SoundManager.instance.SFXPlay("rifle1", rifleClips[1]);
        }
        else if (soundType == "reload")
        {
            SoundManager.instance.SFXPlay("rifleReload", rifleClips[2]);
        }
    }
    public IEnumerator Reload()
    {
        if (stats.RifleAmmo == stats.RifleOriginAmmo || stats.RifleMaxAmmo == 0)
        {
            yield return null;
        }
        else if (stats.RifleMaxAmmo < stats.RifleOriginAmmo - stats.RifleAmmo)
        {
            shooting.IsReloading = true;
            Sound(soundType[2]);
            yield return new WaitForSeconds(ReloadSpeed);
            stats.RifleAmmo += stats.RifleMaxAmmo;
            stats.RifleMaxAmmo = 0;
            shooting.IsReloading = false;
        }
        else
        {
            shooting.IsReloading = true;
            Sound(soundType[2]);
            yield return new WaitForSeconds(ReloadSpeed);
            stats.RifleMaxAmmo -= (stats.RifleOriginAmmo - stats.RifleAmmo);
            stats.RifleAmmo += stats.RifleOriginAmmo - stats.RifleAmmo;
            shooting.IsReloading = false;
        }
        if (gameObject.activeInHierarchy)
        {
            SetUI();
        }
    }
    float nextfire_0;
    float nextfire_1;
    public void Shoot_0()
    {
        IShootable shootableRifle = GetComponent<IShootable>();
        if (Time.time > nextfire_0 && canshoot)
        {
            nextfire_0 = Time.time + ShootDelay_0;
            if (stats.RifleAmmoCost_0 > stats.RifleAmmo)
            {
                if (!shooting.IsReloading)
                {
                    StartCoroutine(shootableRifle.Reload());
                    return;
                }
            }
            stats.RifleAmmo -= stats.RifleAmmoCost_0;
            SetUI();
            // 총쏘는거 구현
            //에임기반 방향정하기
            Sound(soundType[0]);
            RaycastHit hit = new RaycastHit();
            firePos[0].GetComponent<LineRenderer>().SetPosition(0, firePos[0].position);
            if (Physics.Raycast(firePos[0].position, RandomDirection(firePos[0]).normalized, out hit, Distance))
            {
                firePos[0].GetComponent<LineRenderer>().SetPosition(1, hit.point);
                //Damage Method
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(Damage_0);
                }
            }
            else
            {
                firePos[0].GetComponent<LineRenderer>().
                    SetPosition(1, firePos[0].position + (RandomDirection(firePos[0]).normalized * Distance));
            }
            StartCoroutine(LineDuration(false)); //!modChange
        }
    }
    float inaccuarcy = 0.2f; // 샷건 탄분포
    //샷건 탄분포 방향
    Vector3 RandomDirection(Transform fire)
    {
        Vector3 targetPos = fire.position + fire.forward * Distance;
        targetPos = new Vector3
            (
                targetPos.x + Random.Range(-inaccuarcy, inaccuarcy),
                targetPos.y + Random.Range(-inaccuarcy, inaccuarcy),
                targetPos.z + Random.Range(-inaccuarcy, inaccuarcy)
            );
        Vector3 direction = targetPos - fire.position;
        return direction;
    }
    IEnumerator LineDuration(bool modChange)
    {
        if (!modChange)
        {
            firePos[0].GetComponent<LineRenderer>().enabled = true;
            yield return new WaitForSeconds(ShootDelay_0 / 2);
            firePos[0].GetComponent<LineRenderer>().enabled = false;
        }
        else
        {
            firePos[0].GetComponent<LineRenderer>().enabled = true;
            yield return new WaitForSeconds(ShootDelay_1 / 4);
            firePos[0].GetComponent<LineRenderer>().enabled = false;
        }
    }
    public void Shoot_1()
    {
        IShootable shootableRifle = GetComponent<IShootable>();
        if (Time.time > nextfire_1 && canshoot)
        {
            nextfire_1 = Time.time + ShootDelay_1;
            if (stats.RifleAmmoCost_1 > stats.RifleAmmo)
            {
                if (!shooting.IsReloading)
                {
                    StartCoroutine(shootableRifle.Reload());
                    return;
                }
            }
            stats.RifleAmmo -= stats.RifleAmmoCost_1;
            SetUI();
            // 총쏘는거 구현
            //에임기반 방향정하기
            Sound(soundType[1]);
            RaycastHit hit = new RaycastHit();
            firePos[0].GetComponent<LineRenderer>().SetPosition(0, firePos[0].position);
            if (Physics.Raycast(firePos[0].position, firePos[0].forward * Distance, out hit, Distance))
            {
                firePos[0].GetComponent<LineRenderer>().SetPosition(1, hit.point);
                //Damage Method
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(Damage_1);
                }
            }
            else
            {
                firePos[0].GetComponent<LineRenderer>().
                    SetPosition(1, firePos[0].position + (firePos[0].forward * Distance));
            }
            StartCoroutine(LineDuration(true));
        }
    }
    private void Awake()
    {
        Damage_0 = 60;
        Damage_1 = 400;
        ShootDelay_0 = 0.15f;
        ShootDelay_1 = 1.5f;
        ReloadSpeed = 2f;
        Distance = 8f;
    }
    private void OnDisable()
    {
        firePos[0].GetComponent<LineRenderer>().enabled = false;
        StopAllCoroutines();
        canshoot = false;
    }
    bool canshoot = false;
    private void OnEnable()
    {
        Invoke("SwapDelay", 0.4f);
        SetUI();
    }
    void SwapDelay()
    {
        canshoot = true;
    }
    public void SetUI()
    {
        UI.Instance.SetAmmo(stats.RifleAmmo,stats.RifleMaxAmmo);
    }
}
