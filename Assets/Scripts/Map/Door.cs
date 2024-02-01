using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;
    int isOpen = 0;
   public int IsOpen { get { return isOpen; }
        set 
        {   isOpen = value; 
            if( isOpen == 1)
            {
                animator.SetTrigger("open");
            }
            else if(isOpen == 2)
            {
                animator.SetTrigger("close");
            }
        } 
    }
    void Start()
    {
        IsOpen = 0; 
    }
}
