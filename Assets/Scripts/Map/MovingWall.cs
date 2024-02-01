using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingWall : MonoBehaviour
{
    protected bool vertical;
    public float verticalMin;
    public float verticalMax;
    
    protected bool horizontal;
    public float horizontalMin;
    public float horizontalMax;
    public float speed;
    void Update()
    {
        //Vertical
        //-1 ���� �۰ų� �������� �ö󰡰Բ�
        if (transform.position.z <= verticalMin)
        {
            vertical = true;
        }
        //3���� Ŀ���ų� �������� �������Բ�
        else if (transform.position.z >= verticalMax)
        {
            vertical = false;
        }

        if (vertical)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        //================================Horizontal
        
        if (transform.position.x <= horizontalMin)
        {
            horizontal = true;
        }
        else if (transform.position.x >= horizontalMax)
        {
            horizontal = false;
        }

        if (horizontal)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
    }
}
