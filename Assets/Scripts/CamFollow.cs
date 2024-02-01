using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform Player;
    public Vector3 cameraOffset;
    [Range(0.01f, 1.0f)]
    public float smoothness = 0.8f;
    private Quaternion targetRotation;
    void Start()
    {
        targetRotation = transform.rotation;
        cameraOffset = Vector3.zero;
    }
    bool isRotating = false;
    private void Update()
    {
        //ī�޶�� �÷��̾� �����ִ��� Ȯ��
        if (transform.rotation == targetRotation)
        {
            isRotating = false;
        }
        else
        {
            isRotating = true;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q) && !isRotating)
        {
            Player.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));
            targetRotation *= Quaternion.Euler(new Vector3(0, 90, 0));
        }
        if (Input.GetKey(KeyCode.E) && !isRotating)
        {
            Player.rotation *= Quaternion.Euler(new Vector3(0, -90, 0));
            targetRotation *= Quaternion.Euler(new Vector3(0, -90, 0));
        }
        //ī�޶� �ε巴�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothness *0.1f);
    }
    private void LateUpdate()
    {
        //ī�޶� ĳ���Ϳ��� �����¿� ���� �̵�
        if(Player != null)
        {
            Vector3 newPos = cameraOffset + Player.position;
            transform.position = Vector3.Lerp(transform.position, newPos, smoothness);
        }
    }
}
