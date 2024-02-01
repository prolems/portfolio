using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IDamageable
{
    //public static Stats Instance;
    [SerializeField] ParticleSystem fxBlood;
    //private void Awake()
    //{
    //    if(Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(Instance);
    //    }
    //}
    public void SetSGUI()
    {
        UI.Instance.SetAmmo(ShotgunAmmo, ShotgunMaxAmmo);
    }
    public void SetRFUI()
    {
        UI.Instance.SetAmmo(RifleAmmo, RifleMaxAmmo);
    }
    public float HP { get; set; }
    public float Shield { get; set; }
    public int RifleAmmo {get;set;}
    public int RifleOriginAmmo {get;set;}
    public int RifleMaxAmmo { get; set; }
    public int RifleAmmoCost_0 { get; set; }
    public int RifleAmmoCost_1 { get; set; }


    public int ShotgunAmmo { get; set; }
    public int ShotgunOriginAmmo { get; set; }
    public int ShotgunMaxAmmo { get; set; }
    public int ShotgunAmmoCost_0 { get; set; }
    public int ShotgunAmmoCost_1 { get; set; }


    public enum Curgun
    {
        Shotgun,
        Rifle,
    }
    public Curgun curGun = Curgun.Shotgun;
    public void Damage(float dmg)
    {
        if (Shield >= dmg)
        {
            Shield -= dmg;

        }
        else if (Shield < dmg)
        {
            // 100 - 400 = -300
            Shield -= dmg;
            HP += Shield;
            if (HP <= 0)
            {
                // Dead Method
                Dead();
            }
            Shield = 0;
        }
        // UI hp/sheild  나타내기
        UI.Instance.SetHP(HP, Shield);
    }
    void Start()
    {
        HP = 100;
        Shield = 100;
        //UI hp shield 나타내기
        UI.Instance.SetHP(HP, Shield);
        //샷건 탄 데이터
        ShotgunAmmo               = 10;
        ShotgunOriginAmmo         = 10;
        ShotgunMaxAmmo            = 50;
        ShotgunAmmoCost_0         = 1;
        ShotgunAmmoCost_1 = 3;
        //라이플 탄 데이터
        RifleAmmo = 60;
        RifleOriginAmmo = 60;
        RifleMaxAmmo = 120;
        RifleAmmoCost_0 = 1;
        RifleAmmoCost_1 = 10;
    }
    void Dead()
    {
        fxBlood.Play();
        Destroy(gameObject, 0.5f);
        UI.Instance.state = UI.State.Stop;
        UI.Instance.failedPanel.SetActive(true);

    }
}
