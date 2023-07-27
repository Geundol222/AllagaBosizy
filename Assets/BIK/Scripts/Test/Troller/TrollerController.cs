using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class TrollerController : MonoBehaviourPunCallbacks
{
    [SerializeField] Texture2D cursor;
    [SerializeField] float cameraMoveSpeed;
    [SerializeField] float padding;
    Vector3 cameraMoveDir;
    bool pressArrows = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;                         // ���콺 Ŀ�� ���� ������ �ȿ����� 
        Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);   // ���콺 Ŀ�� �̹��� �ֱ�
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        transform.Translate(Vector3.right * cameraMoveDir.x * cameraMoveSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * cameraMoveDir.y * cameraMoveSpeed * Time.deltaTime, Space.World);
    }

    private void OnMove(InputValue value) // ����Ű �Է� ����
    {
        // ����Ű �Է��� Ȯ���Ͽ� pressArrows�� ���� ���������� ����
        // OnPointer�ʿ��� ����Ű �Է� ���� ��쿣 cameraMoveDir �� 0���� ������ ���ϰ� ó��
        Vector2 keyboardPos = value.Get<Vector2>();
        if (keyboardPos.x != 0 || keyboardPos.y != 0) // ����Ű �Է� ����
        {
            pressArrows = true;
            Debug.Log("������.");
        } else
        {
            pressArrows = false;
        }

        cameraMoveDir = keyboardPos;
    }

    private void OnPointer(InputValue value) // ���콺 Ŀ�� �̵�����
    {
        // ���� ���� ����Ű�� ī�޶� ��ȯ�� �̷����� �ִٸ�, ���콺�� �̿��� ī�޶� �̵��� �Ұ����ϰ� ó��
        if (pressArrows)
            return;

        Vector2 mousePos = value.Get<Vector2>();
        Debug.Log($"x {mousePos.x}  y {mousePos.y} ");

        if(mousePos.x <= padding)
        {
            Debug.Log("��������");
            cameraMoveDir.x = -1;
        } else if(mousePos.x >= Screen.width - padding)
        {
            Debug.Log("����������");
            cameraMoveDir.x = 1;
        }
        else
        {
            cameraMoveDir.x = 0;
        }

        if(mousePos.y <= padding) // ���콺�� y ��ġ ���� �е� ������ ���ų� �۴�.
        {
            cameraMoveDir.y = -1;
        } else if(mousePos.y >= Screen.height - padding) // ���콺�� y ��ġ ���� �ֻ���� �е� ���� �ش��ϴ� �����̴�.
        {
            cameraMoveDir.y = 1;
        } else
        {
            cameraMoveDir.y = 0;
        }        
        
    }


}
