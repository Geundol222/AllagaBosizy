using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrollerCameraController : MonoBehaviourPunCallbacks
{
    [SerializeField] Texture2D cursor;
    [SerializeField] float cameraMoveSpeed;
    [SerializeField] float padding;
    [SerializeField] CinemachineVirtualCamera vcam;

    Vector3 cameraMoveDir;
    bool pressArrows = false;
    bool foundCamera = false;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;                         // ���콺 Ŀ�� ���� ������ �ȿ����� 
        //Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);   // ���콺 Ŀ�� �̹��� �ֱ�
        StartCoroutine(FindCameraCoroutine());
    }

    IEnumerator FindCameraCoroutine()
    {
        yield return new WaitUntil(() => { return vcam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>(); });
        foundCamera = true;
        vcam.transform.position = new Vector3(transform.position.x, transform.position.y, vcam.transform.position.z);
        yield break;
    }


    public override void OnDisable()
    {
        base.OnDisable();
        Cursor.lockState = CursorLockMode.None;
        StopAllCoroutines();
    }

    private void LateUpdate()
    {
        if (foundCamera && photonView.IsMine)
            CameraMove();
    }

    private void FixedUpdate()
    {
        if (foundCamera && photonView.IsMine)
            CheckCameraInCamZone();
    }

    /// <summary>
    /// �ó׸ӽ� ī�޶� CamZone ���� ���� �ִ��� Ȯ�����ִ� �Լ�
    /// ���� ������ �������� ���� ���� ��ġ������ �̵� ���� 0����
    /// </summary>
    private void CheckCameraInCamZone()
    {
        if (vcam.transform.position.x <= -16.5 || vcam.transform.position.x >= 16)
        {
            cameraMoveDir.x = 0;
        }

        if (vcam.transform.position.y <= -0.5 || vcam.transform.position.y >= 168)
        {
            cameraMoveDir.y = 0;
        }
    }

    private void CameraMove()
    {
        vcam.transform.Translate(Vector3.right * cameraMoveDir.x * cameraMoveSpeed * Time.deltaTime, Space.World);
        vcam.transform.Translate(Vector3.up * cameraMoveDir.y * cameraMoveSpeed * Time.deltaTime, Space.World);
    }

    private void OnMove(InputValue value) // ����Ű �Է� ����
    {
        // ����Ű �Է��� Ȯ���Ͽ� pressArrows�� ���� ���������� ����
        // OnPointer�ʿ��� ����Ű �Է� ���� ��쿣 cameraMoveDir �� 0���� ������ ���ϰ� ó��
        Vector2 keyboardPos = value.Get<Vector2>();
        if (keyboardPos.x != 0 || keyboardPos.y != 0) // ����Ű �Է� ����
        {
            pressArrows = true;
        }
        else
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

        if (-10 < mousePos.x && mousePos.x <= 0 + padding)
        {
            cameraMoveDir.x = -1;
        }
        else if (mousePos.x >= Screen.width - padding && mousePos.x <= Screen.width + 10)
        {
            cameraMoveDir.x = 1;
        }
        else
        {
            cameraMoveDir.x = 0;
        }

        if (-10 < mousePos.y && mousePos.y <= 0 + padding) // ���콺�� y ��ġ ���� �е� ������ ���ų� �۴�.
        {
            cameraMoveDir.y = -1;
        }
        else if (mousePos.y >= Screen.height - padding && mousePos.y <= Screen.height + 10) // ���콺�� y ��ġ ���� �ֻ���� �е� ���� �ش��ϴ� �����̴�.
        {
            cameraMoveDir.y = 1;
        }
        else
        {
            cameraMoveDir.y = 0;
        }
    }


}
