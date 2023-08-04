using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Platform : MonoBehaviourPun,IPunObservable
{
    [SerializeField] private TrollerPlayerController trollerPlayerController;
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;
    [SerializeField] private TMP_Text currentStateText;
    [SerializeField] private Slider countDownSlider;

    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount; 
    private Color pointerOutColor = Color.white;
    private SetTrapUI setTrapUI;
    public SetTrapUI _setTrapUI { get { return setTrapUI; } }
    private UICloseArea closeArea;

    private Debuff platformCurrentDebuff;
    private Debuff_State currentDebuffState;
    public Debuff_State _currentDebuffState {  get { return currentDebuffState; } }

    private Coroutine debuffCountDownCoroutine;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            Debug.Log("�����ƴ�");
            Destroy(gameObject);

        }
        else
        {
            Debug.Log("������");
        }
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;

        trollerPlayerController = GameObject.Find("TrollerController(Clone)").GetComponent<TrollerPlayerController>(); // ���� DataManager�� �����Ǿ����
        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        platformCurrentDebuff = trollerPlayerController.CreateNoneStateDebuff();
        currentDebuffState = platformCurrentDebuff.state;
    }

    [PunRPC]
    public void UpdateCurrentStateText()
    {
        string text = "";
        if(currentDebuffState != Debuff_State.None)
        {
            text = currentDebuffState.ToString();
        }
        currentStateText.text = text;
    }

    public void DebuffQueueEnqueue()
    {
        trollerPlayerController.DebuffQueueEnqueue();
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

    /// <summary>
    /// ���� ��ġ ��ư �����ֱ�
    /// </summary>
    public void ShowSetTrapButton()
    {
        if (!photonView.IsMine)
            return;
        isClickable = false;

        //1. Ŭ���� �÷����� Ʈ�ѷ� ��Ʈ�ѷ��� ���� �÷������� ����
        SetCurrentPlatform();

        //1-2. Ȥ�� ���� �÷����� NULL�̸� ���� �÷����� ���� �÷������� ����
        if (trollerPlayerController._prevPlatform == null)
        {
            SetPrevPlatform();
        }

        //2. ���� �÷����� ���� �÷����� �ٸ��ٸ� ���� �÷����� �ݾ���.
        if (trollerPlayerController._currentPlatform != trollerPlayerController._prevPlatform)
        {
            trollerPlayerController._prevPlatform._setTrapUI.ExecuteSetTrapButtonClosing();
            //SetTrapUI�� �����Ǿ� Platform�� �����ϸ� ��ũ��Ʈ ���ο��� CloseArea�� Platform�� �ʱ�ȭ ���ִ� ������ �ٲ� - 230801 02:19 AM 
            //trollerPlayerController._prevPlatform.ClearCloseAreaPlatform();
        }

        //3. ���� �÷����� NULL�� �ٲ��ְ�
        ClearCurrentPlatform();
        //4. �� �÷����� ���� ���� �÷������� ..
        SetPrevPlatform();

        //SetTrapUI�� �����Ǿ� Platform�� �����ϸ� ��ũ��Ʈ ���ο��� CloseArea�� Platform�� �ʱ�ȭ ���ִ� ������ �ٲ� - 230801 02:19 AM 
        //InitToUICloseArea(); 

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllBufferedViaServer);
        }

        setTrapUI = GameManager.UI.ShowInGameUI<SetTrapUI>("UI/SetTrapButton");
        setTrapUI.SetParentPlatform(this);
        setTrapUI.SetTarget(transform);
        setTrapUI.SetOffset(new Vector3(200, 0));
    }

    /// <summary>
    /// ���� ��ġ ��ư �����
    /// </summary>
    public void HideSetTrapButton()
    {
        if (!photonView.IsMine)
            return;

        if (setTrapUI == null)
            return;

        isClickable = true;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerExitPlatform", RpcTarget.AllBufferedViaServer);
        }

        //SetTrapUI ��ũ��Ʈ ���ο��� HideSetTrapButton �� CloseArea�� Platform�� �ʱ�ȭ ���ִ� ������ �ٲ� - 230801 02:19 AM  
        //ClearCloseAreaPlatform();
        GameManager.UI.CloseInGameUI(setTrapUI);
    }
      
    /// <summary>
    /// �÷��̾ ���ǿ� ��Ҵ�. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�÷��̾� ����");
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // ������� ������ return
            if (platformCurrentDebuff == null || platformCurrentDebuff.state == Debuff_State.None)
                return;

            // ���� �÷����� ������ ��ġ�ϴ� �Լ� ȣ��
            if(currentDebuffState == Debuff_State.NoColider)
                platformCurrentDebuff.SetTrap(this);
            StartDebuffCountDown();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            CallRPCFunction("PlayerEnteredPlatform");
            CallRPCFunction("SwitchRenderColorEnter");
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
            CallRPCFunction("PlayerExitPlatform");
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

    public void OnClickSetTrap()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            CallRPCFunction("SetTrap");
        }
    }
      
    [PunRPC]
    public void SetTrap()
    {
        // ����� ť���� �ϳ� ������ Dequeue();
        // ���� �÷����� ������� ����
        platformCurrentDebuff = (Debuff) trollerPlayerController.debuffQueue.Dequeue();
        // ���� �÷����� DebuffState ������Ʈ 
        currentDebuffState = platformCurrentDebuff.state;
        // �ؽ�Ʈ ������Ʈ 
        CallRPCFunction("UpdateCurrentStateText");
        // NoCollider�� �ƴ϶������ �÷����� ������ ��ġ�ϴ� �Լ� ȣ��
        if (currentDebuffState != Debuff_State.NoColider)
            platformCurrentDebuff.SetTrap(this);
        // ����� ���Կ� ���� ����� �ϳ� �߰����ֱ�
        DebuffQueueEnqueue();
        // ��������Ʈ �����ϱ�

        Debug.Log(platformCurrentDebuff.state);
    }

    [PunRPC]
    public void ClearTrap()
    {
        platformCurrentDebuff = trollerPlayerController.CreateNoneStateDebuff();
        currentDebuffState = platformCurrentDebuff.state;
        CallRPCFunction("UpdateCurrentStateText");
        platformCurrentDebuff.SetTrap(this);
        countDownSlider.value = 1;
        countDownSlider.gameObject.SetActive(false);
        debuffCountDownCoroutine = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)currentDebuffState);
        }
        else
        {
            currentDebuffState = (Debuff_State) stream.ReceiveNext();
        }
    }

    public void StartDebuffCountDown()
    { 
        countDownSlider.gameObject.SetActive(true);
        if (debuffCountDownCoroutine == null)
        {
            debuffCountDownCoroutine = StartCoroutine(CountDownCoroutine());
        }
    }

    public IEnumerator CountDownCoroutine()
    {
        float time = 0;
        while(currentDebuffState != Debuff_State.None)
        {
            yield return new WaitForSeconds(1f);
            countDownSlider.value -= 0.2f;
            time++;
            if(time >= 5)
            {
                CallRPCFunction("ClearTrap");
                yield return null;
            }
                     
        }
    }

    public void ClearDebuff()
    {
        if(playerCount != 0)
        {
            return;
        }
    }

    public void CallRPCFunction(string functionName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC(functionName, RpcTarget.AllBufferedViaServer);
        }
    }
}
