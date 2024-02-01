using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Stats stats;
    public bool IsReloading { get; set; }
    bool modeChange = false;

    public bool[] Havegun = new bool[5];
    private void Update()
    {
        if (UI.Instance.state == UI.State.Stop)
            return;

        // 특정키(1,2,3,4,5)를 눌렀을때 해당하는 무기가있다면(Havegun) 총을 들기
        if (Input.GetKeyDown(KeyCode.Alpha1) && Havegun[0])
        {
           stats.curGun = Stats.Curgun.Shotgun;
            Swap(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && Havegun[1])
        {
            stats.curGun = Stats.Curgun.Rifle;
            Swap(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && Havegun[2])
        {
            Swap(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && Havegun[3])
        {
            Swap(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && Havegun[4])
        {
            Swap(4);
        }
        /////////////////////////////////////////////////
        ///모든 총들 발사와 장전 구현, 총에따른 데이터및 ui변화
        if (curGun.Count != 0)
        {
            GameObject gun = curGun.Peek(); //curgun 첫번째index 반환
            IShootable shootable = gun.GetComponent<IShootable>();
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!IsReloading)
                {
                    StartCoroutine(shootable.Reload());
                }
            }
            if (Input.GetMouseButtonDown(1) && !IsReloading)
            {
                modeChange = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                modeChange = false;
            }
            if (Input.GetMouseButton(0) && !IsReloading)
            {
                if (!modeChange)
                {
                    shootable.Shoot_0();
                }
                else if (modeChange)
                {
                    shootable.Shoot_1();
                }
            }
        }
    }
    public GameObject[] guns;
    public Queue<GameObject> curGun = new Queue<GameObject>();
    void Swap(int i)
    {
        foreach (var gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
        if (Havegun[i])
        {
            guns[i].gameObject.SetActive(true);
            if (curGun.Count == 0)
            {
                curGun.Enqueue(guns[i]);
            }
            else
            {
                curGun.Enqueue(guns[i]);
                curGun.Dequeue();
            }
        }
    }
}
