using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Platform : MonoBehaviourPun,IPunObservable
{
    private TrollerPlayerController trollerPlayerController { get { return GameManager.TrollerData.trollerPlayerController; } set { GameManager.TrollerData.trollerPlayerController = value;  } }
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;
    [SerializeField] private TMP_Text currentStateText;
    [SerializeField] private Slider countDownSlider;

    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount; 
    private Color pointerOutColor = Color.white;
    private UICloseArea closeArea;

    private SetTrapUI setTrapUI;
    public SetTrapUI _setTrapUI { get { return setTrapUI; } set { setTrapUI = value; } }

    private Debuff platformCurrentDebuff;
    public Debuff_State currentDebuffState {  get { return platformCurrentDebuff.state; } set { platformCurrentDebuff.state = value;  } }

    private Coroutine debuffCountDownCoroutine;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            Destroy(gameObject);

        }
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;
        
        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        platformCurrentDebuff = GameManager.Debuff.CreateNoneStateDebuff();
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
        GameManager.Debuff.DebuffQueueEnqueue();
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

    public void ShowSetTrapUI(Platform platform)
    {
        setTrapUI = GameManager.UI.ShowInGameUI<SetTrapUI>("UI/SetTrapButton");
        setTrapUI.SetParentPlatform(platform);
        setTrapUI.SetTarget(transform);
        setTrapUI.SetOffset(new Vector3(200, 0));
    }

    public void HideSetTrapUI()
    {
        GameManager.UI.CloseInGameUI(setTrapUI);
    }
    /// <summary>
    /// ���� ��ġ ��ư �����ֱ�
    /// </summary>
    public void ShowSetTrapButton()
    {
        isClickable = false;

        //1. Ŭ���� �÷����� Ʈ�ѷ� ��Ʈ�ѷ��� ���� �÷������� ����
        SetCurrentPlatform();

        //1-2. Ȥ�� ���� �÷����� NULL�̸� ���� �÷����� ���� �÷������� ����
        if (GameManager.TrollerData.prevPlatform == null)
        {
            SetPrevPlatform();
        }

        //2. ���� �÷����� ���� �÷����� �ٸ��ٸ� ���� �÷����� �ݾ���.
        if (GameManager.TrollerData.currentPlatform != GameManager.TrollerData.prevPlatform)
        {
            GameManager.TrollerData.prevPlatform._setTrapUI.ExecuteSetTrapButtonClosing();
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

        Debug.Log("�� ȣ��");
        ShowSetTrapUI(this);
    }

    /// <summary>
    /// ���� ��ġ ��ư �����
    /// </summary>
    public void HideSetTrapButton()
    {
        if (setTrapUI == null)
            return;

        isClickable = true;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerExitPlatform", RpcTarget.AllBufferedViaServer);
        }

        //SetTrapUI ��ũ��Ʈ ���ο��� HideSetTrapButton �� CloseArea�� Platform�� �ʱ�ȭ ���ִ� ������ �ٲ� - 230801 02:19 AM  
        //ClearCloseAreaPlatform();
        HideSetTrapUI();
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
            if (platformCurrentDebuff == null || currentDebuffState == Debuff_State.None)
                return;

            // ���� �÷����� ��ġ�� ������ �����ϴ�
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
        if (isClickable == false)
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
        platformCurrentDebuff = (Debuff) GameManager.TrollerData.debuffQueue.Dequeue();
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
        platformCurrentDebuff = GameManager.Debuff.CreateNoneStateDebuff();
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
