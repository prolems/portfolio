using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMag : Item
{
    public int MagIndex { get; set; }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>() == stats)
        {
            // 아이템의 종류에 따라 플레이어 총알 늘리기
            switch (MagIndex)
            {
                case 0:
                    stats.ShotgunMaxAmmo+= 10;
                    if(stats.curGun == Stats.Curgun.Shotgun)
                    {
                        stats.GetComponent<Stats>().SetSGUI();
                    }
                    break;
                case 1:
                    stats.RifleMaxAmmo += 20;
                    if(stats.curGun == Stats.Curgun.Rifle)
                    {
                        stats.GetComponent<Stats>().SetRFUI();
                    }
                    break;
            }
            Destroy(gameObject);
        }
    }
    public void SetStats(Stats stats)
    {
        this.stats = stats;
    }
}
