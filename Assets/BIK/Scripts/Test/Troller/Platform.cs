using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviourPun
{
    [SerializeField] private TrollerPlayerController trollerPlayerController;
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;
    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount;
    private Color pointerOutColor = Color.white;

    private SetTrapUI setTrapUI;
    public SetTrapUI _setTrapUI {  get { return setTrapUI;  } }

    private UICloseArea closeArea;

    private void Awake()
    {
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;

        trollerPlayerController = GameObject.Find("TrollerController").GetComponent<TrollerPlayerController>(); // ���� DataManager�� �����Ǿ����
        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void InitToUICloseArea()
    {
        Debug.Log("InitToUICloseArea");
        closeArea.Init(this);
    }

    public void ClearCloseAreaPlatform()
    {
        closeArea.ClearPlatform();
    }

    public void ClearBothPlatform()
    {
        trollerPlayerController.ClearBothPlatform();
    }

    public void ClearCurrentPlatform()
    {
        trollerPlayerController.ClearCurrentPlatform();
    }

    public void SetCurrentPlatform()
    {
        trollerPlayerController.SetCurrentPlatform(this);
    }

    public void SetPrevPlatform()
    {
        trollerPlayerController.SetPrevPlatform(this);
    }

    public void ShowSetTrapButton()
    {
        if (!photonView.IsMine)
            return;

        //1. Ŭ���� �÷����� Ʈ�ѷ� ��Ʈ�ѷ��� ���� �÷������� ����
        SetCurrentPlatform(); 

        //1-2. Ȥ�� ���� �÷����� NULL�̸� ���� �÷����� ���� �÷������� ����
        if(trollerPlayerController._prevPlatform == null)
        {
            SetPrevPlatform();
        }

        //2. ���� �÷����� ���� �÷����� �ٸ��ٸ� ���� �÷����� �ݾ���.
        if(trollerPlayerController._currentPlatform != trollerPlayerController._prevPlatform)
        {
            trollerPlayerController._prevPlatform._setTrapUI.ExecuteSetTrapButtonClosing();
            //trollerPlayerController._prevPlatform.ClearCloseAreaPlatform();
        }

        //3. ���� �÷����� NULL�� �ٲ��ְ�
        ClearCurrentPlatform();
        //4. �� �÷����� ���� ���� �÷������� ..
        SetPrevPlatform();

        InitToUICloseArea(); // ���῵������ Ŭ���� ���� �Ǿ���� Platform �����ϰ� 


        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllBufferedViaServer);
        }

        setTrapUI = GameManager.UI.ShowInGameUI<SetTrapUI>("UI/SetTrapButton");
        setTrapUI.SetParentPlatform(this);
        setTrapUI.SetTarget(transform);
        setTrapUI.SetOffset(new Vector3(200, 0));
    }

    public void HideSetTrapButton()
    {
        if (!photonView.IsMine)
            return;

        if (setTrapUI == null)
            return;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerExitPlatform", RpcTarget.AllBufferedViaServer);
        }

        // SetTrapUI������ �ڷ�ƾ ������� ���� ��߳��� �� �ϴ� UICloseArea ��ũ��Ʈ�� Plarform�� ���� Null ���� �ʰ� ���ο� Platform���� Update ���ִ� ������� �����ϰ���.
        //ClearCloseAreaPlatform();
        GameManager.UI.CloseInGameUI(setTrapUI);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllBufferedViaServer);
            photonView.RPC("SwitchRenderColorEnter", RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    private void PlayerEnteredPlatform()
    {
        playerCount++;
        if (playerCount > 0)
            isClickable = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        if (PhotonNetwork.IsConnectedAndReady)
            photonView.RPC("PlayerExitPlatform", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void PlayerExitPlatform()
    {
        playerCount--;
        if (playerCount <= 0)
        {
            playerCount = 0;
            isClickable = true;
        }
    }

    [PunRPC]
    public void SwitchRenderColorEnter()
    {
        if (!photonView.IsMine || isClickable == false)
            return;        

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
                renderer.material.color = pointerOverColor;
        }
    }

    [PunRPC]
    public void SwitchRenderColorExit()
    {
        if (!photonView.IsMine)
            return;

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
                renderer.material.color = pointerOutColor;
        }
    }
}
