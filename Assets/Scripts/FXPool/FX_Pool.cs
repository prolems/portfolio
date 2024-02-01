using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Pool : MonoBehaviour
{
    public static FX_Pool instance;
    // [SerializeField] Player p;
    [SerializeField] Transform player;
    [SerializeField] Stats stats;
    [SerializeField] FX_Jump fxJump;
    [SerializeField] Transform parent;
    [SerializeField] ShotGun sg;
    Queue<FX_Jump> fxJumpQ = new Queue<FX_Jump>();
    [SerializeField] ShotgunGranade sgg;
     Queue<ShotgunGranade> granadeQ = new Queue<ShotgunGranade>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        fxJumpQ.Clear();
    }
    public void SGGranade(Transform firePos)
    {
        ShotgunGranade sgg;
        if (granadeQ.Count == 0)
        {
            sgg = Instantiate(this.sgg, firePos);
            sgg.SetShotgun(sg);
            sgg.transform.SetParent(null);
            granadeQ.Enqueue(sgg);
        }
        else
        {
            sgg = granadeQ.Dequeue();
            sgg.transform.position = firePos.position;
            sgg.transform.localRotation = firePos.rotation;
            sgg.gameObject.SetActive(true);
        }
    }
    public void SGGranadeEnd(ShotgunGranade sgg)
    {
        granadeQ.Enqueue(sgg);
        sgg.transform.SetParent(parent);
        sgg.gameObject.SetActive(false);
    }
    public void FxJump(Vector3 playerPos)
    {
        FX_Jump fxJ;

        if (fxJumpQ.Count == 0)
        {
            fxJ = Instantiate(fxJump, playerPos, Quaternion.identity);
            fxJ.gameObject.SetActive(true);
            fxJ.transform.SetParent(parent);
            fxJumpQ.Enqueue(fxJ);
        }
        else
        {
            fxJ = fxJumpQ.Dequeue();
            fxJ.transform.position = player.transform.position;
            fxJ.gameObject.SetActive(true);
        }
    }
    public void FxJumpEnd(FX_Jump fxJump)
    {
        fxJumpQ.Enqueue(fxJump);
        fxJump.gameObject.SetActive(false);
    }
}
