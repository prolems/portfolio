using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShotGun : Gun, IShootable
{
    [SerializeField] private AudioClip[] clips;
    float nextfire_0;
    float nextfire_1;
    //¼¦°Ç ±âº»¹ß»ç
    public void Shoot_0()
    {
        IShootable shootableShotgun = GetComponent<IShootable>();
        if (Time.time > nextfire_0 && canshoot)
        {
            nextfire_0 = Time.time + ShootDelay_0;
            if (stats.ShotgunAmmoCost_0 > stats.ShotgunAmmo)
            {
               //ÅººÎÁ·
                if (!shooting.IsReloading)
                {
                    StartCoroutine(shootableShotgun.Reload());
                    return;
                }
            }
            stats.ShotgunAmmo -= stats.ShotgunAmmoCost_0;
            SetUI();
            // ÃÑ½î´Â°Å ±¸Çö
            //¿¡ÀÓ±â¹Ý ¹æÇâÁ¤ÇÏ±â
            string fire0 = "fire0";
            StartCoroutine(Sound(fire0));
            RaycastHit[] hits = new RaycastHit[5];

            foreach (Transform item in firePos)
            {
                item.GetComponent<LineRenderer>().SetPosition(0, item.position);
            }

            int i = 0;
            
            foreach (Transform fire in firePos)
            {

                if (Physics.Raycast(fire.position, RandomDirection(fire).normalized, out hits[i], Distance))
                {
                    fire.GetComponent<LineRenderer>().SetPosition(1, hits[i].point);
                    //Damage Method
                    IDamageable damageable = hits[i].collider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.Damage(Damage_0);
                    }
                }
                else
                {
                    fire.GetComponent<LineRenderer>().SetPosition(1, fire.position + (RandomDirection(fire).normalized * Distance));
                }
                i++;
            }
            StartCoroutine(LineDuration());
        }
    }
    float inaccuarcy = 0.4f; // ¼¦°Ç ÅººÐÆ÷
    //¼¦°Ç ÅººÐÆ÷ ¹æÇâ
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
    //¼¦°Ç Åº±ËÀû LineRenderer
    IEnumerator LineDuration()
    {
        foreach (var item in firePos)
        {
            item.GetComponent<LineRenderer>().enabled = true;

        }
        yield return new WaitForSeconds(ShootDelay_0 / 10);
        foreach (var item in firePos)
        {
            item.GetComponent<LineRenderer>().enabled = false;
        }
    }
    IEnumerator Sound(string soundType)
    {
        if (soundType == "fire0")
        {
            SoundManager.instance.SFXPlay("Shotgun0", clips[0]);
            yield return new WaitForSeconds(clips[0].length / 8);
            SoundManager.instance.SFXPlay("ShotgunCoking", clips[1]);
        }
        else if (soundType == "fire1")
        {
            SoundManager.instance.SFXPlay("Shotgun1", clips[2]);
        }
        else if (soundType == "reload")
        {
            SoundManager.instance.SFXPlay("ShotgunReload", clips[3]);
        }
    }
    public void Shoot_1()
    {
        string fire1 = "fire1";
        IShootable shootableShotgun = GetComponent<IShootable>();
        if (Time.time > nextfire_1 && canshoot)
        {
            nextfire_1 = Time.time + ShootDelay_1;
            if (stats.ShotgunAmmoCost_1 > stats.ShotgunAmmo)
            {
               //ÅººÎÁ·
                if (!shooting.IsReloading)
                {
                    StartCoroutine(shootableShotgun.Reload());
                    return;
                }
            }
            stats.ShotgunAmmo -= stats.ShotgunAmmoCost_1;
            //À¯Åº ±¸Çö pool Àû¿ë
            FX_Pool.instance.SGGranade(firePos[0]);
            StartCoroutine(Sound(fire1));
            SetUI();
        }
    }
    IEnumerator IShootable.Reload()
    {
        string reload = "reload";
        if (stats.ShotgunAmmo == stats.ShotgunOriginAmmo || stats.ShotgunMaxAmmo == 0)
        {
            yield return null;
        }

        else if (stats.ShotgunMaxAmmo <stats.ShotgunOriginAmmo- stats.ShotgunAmmo)
        {
            shooting.IsReloading = true;
            StartCoroutine(Sound(reload));
            yield return new WaitForSeconds(ReloadSpeed);
            stats.ShotgunAmmo += stats.ShotgunMaxAmmo;
            stats.ShotgunMaxAmmo = 0;
            shooting.IsReloading = false;
        }
        else
        {
            shooting.IsReloading = true;
            StartCoroutine(Sound(reload));
            yield return new WaitForSeconds(ReloadSpeed);
            stats.ShotgunMaxAmmo -= (stats.ShotgunOriginAmmo - stats.ShotgunAmmo);
            stats.ShotgunAmmo += stats.ShotgunOriginAmmo - stats.ShotgunAmmo;
            shooting.IsReloading = false;
        }
        if (gameObject.activeInHierarchy)
        {
            SetUI();
        }
    }
    private void Awake()
    {
        
        Damage_0 = 64;
        Damage_1 = 400;
        ShootDelay_0 = 1f;
        ShootDelay_1 = 3f;
        ReloadSpeed = 1f;
        Distance = 5f;
    }
    private void OnDisable()
    {
        foreach (var item in firePos)
        {
            item.GetComponent<LineRenderer>().enabled = false;
        }
        StopAllCoroutines();
        canshoot = false;
    }
    private void OnEnable()
    {
        Invoke("SwapDelay", 0.4f);
        SetUI();
    }
    bool canshoot = false;
    void SwapDelay()
    {
        canshoot = true;
    }
    public void SetUI()
    {
        UI.Instance.SetAmmo(stats.ShotgunAmmo, stats.ShotgunMaxAmmo);
    }
   
}
