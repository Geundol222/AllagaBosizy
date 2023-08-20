using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Platform : MonoBehaviourPun, IPunObservable
{
    private TrollerPlayerController trollerPlayerController { get { return GameManager.TrollerData.trollerPlayerController; } set { GameManager.TrollerData.trollerPlayerController = value; } }
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;
    [SerializeField] private TMP_Text currentStateText;
    [SerializeField] private Slider countDownSlider;

    private DebuffManager debuffManager { get { return GameManager.TrollerData.debuffManager; } }

    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount;
    private Color pointerOutColor = Color.white;
    private UICloseArea closeArea;
    private SetTrapUI setTrapUI;
    public SetTrapUI _setTrapUI { get { return setTrapUI; } set { setTrapUI = value; } }

    [SerializeField] Debuff platformCurrentDebuff;
    public Debuff_State currentDebuffState { get { return platformCurrentDebuff.state; } set { platformCurrentDebuff.state = value; UpdateCurrentStateText(); } }

    private Coroutine debuffCountDownCoroutine;
    private Coroutine debuffSetCoolDownCoroutine;

    [SerializeField] private int photonPlayerNumber; // �� �÷����� ���� �ο��� �÷��̾��� Number�� ������ �ֱ�

    private void Awake()
    {
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;

        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
        renderers = GetComponentsInChildren<Renderer>();
        platformCurrentDebuff = (Debuff)GameManager.TrollerData.Original_Debuff.clone();
        UpdateCurrentStateText();
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    [PunRPC]
    public void UpdateCurrentStateText()
    {
        string text = "";

        if (GameManager.Team.GetTeam() != PlayerTeam.Troller)
            return;

        Debug.Log($"���� CurrentPlatformState�� {currentDebuffState.ToString()}");
        if (currentDebuffState != Debuff_State.None)
        {
            text = currentDebuffState.ToString();
        }
        currentStateText.text = text;
    }

    public void DebuffQueueEnqueue()
    {
        debuffManager.DebuffQueueEnqueue();
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
        setTrapUI.SetOffset(new Vector3(0, 0));
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

        if (platformCurrentDebuff.state == Debuff_State.None)
        {
            isClickable = true;
        }

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
            if (currentDebuffState == Debuff_State.NoCollider)
                debuffManager.SetTrap(platformCurrentDebuff, this);
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
            if (platformCurrentDebuff.state == Debuff_State.None)
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
        Debug.Log("Set Trap ��ư ����");
        // ������ġ�� �÷��̾� Number �ֱ�
        photonPlayerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        // ������ġ �Ǵ� ���콺 Over Exit �׼� ���ϰ� ó��
        SetCanMouseAction(false);
        // setTrapPlatform�� ���� �÷��� �߰�,
        if (photonPlayerNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            SetTrapListUpdate(true);
        }
        // ��Ÿ�� ����
        debuffSetCoolDownCoroutine = StartCoroutine(DebuffSetCoolTimeCoroutine((int)GameManager.TrollerData.debuffSetCoolTime));

        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debuff debuff = (Debuff)GameManager.TrollerData.debuffQueue.Dequeue();
            CallRPCFunction("SetTrap", (int)debuff.state);
            // ����� ���Կ� ���� ����� �ϳ� �߰����ֱ�
            if (GameManager.TrollerData.debuffCount < GameManager.TrollerData.debuffQueueLength)
                DebuffQueueEnqueue();
        }
    }

    IEnumerator DebuffSetCoolTimeCoroutine(int cooltime)
    {
        GameManager.TrollerData.debuffManager.ShowCoolTimeUI();
        yield return null;
        while (cooltime > 0)
        {
            GameManager.TrollerData.debuffManager.SetCoolTimeText(cooltime);
            Debug.Log($"��Ÿ�� : {cooltime} ����");
            cooltime--;
            yield return new WaitForSeconds(1f);
        }
        GameManager.TrollerData.debuffManager.HideCoolTimeUI();
        SetCanMouseAction(true);
        yield break;
    }

    [PunRPC]
    public void SetTrap(int debuffIndex)
    {
        isClickable = false;
        Debuff setTrapDebuff = (Debuff)GameManager.TrollerData.Original_Debuff.clone();
        setTrapDebuff.SetState(debuffIndex);
        platformCurrentDebuff = setTrapDebuff;
        CallRPCFunction("UpdateCurrentStateText");
        // NoCollider�� �ƴ϶������ �÷����� ������ ��ġ�ϴ� �Լ� ȣ��
        if (currentDebuffState != Debuff_State.NoCollider)
            debuffManager.SetTrap(platformCurrentDebuff, this);
    }

    [PunRPC]
    public void ClearTrap()
    {
        if (photonPlayerNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            SetTrapListUpdate(false);
        }
        isClickable = true;
        platformCurrentDebuff = debuffManager.CreateNoneStateDebuff();
        //currentDebuffState = platformCurrentDebuff.state;
        CallRPCFunction("UpdateCurrentStateText");
        debuffManager.SetTrap(platformCurrentDebuff, this);
        countDownSlider.value = 1;
        countDownSlider.gameObject.SetActive(false);
        debuffCountDownCoroutine = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)currentDebuffState);
            stream.SendNext((bool)isClickable);
        }
        else
        {
            currentDebuffState = (Debuff_State)stream.ReceiveNext();
            isClickable = (bool)stream.ReceiveNext();
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
        while (currentDebuffState != Debuff_State.None)
        {
            yield return new WaitForSeconds(1f);
            countDownSlider.value -= 0.2f;
            time++;
            if (time >= 5)
            {
                CallRPCFunction("ClearTrap");
                // ���� �� �÷����� ������ '��'�� ��ġ�ߴٸ� ����Ʈ���� �����Ѵ�. 
                yield break;
            }
        }
    }

    public void ClearDebuff()
    {
        if (playerCount != 0)
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

    public void CallRPCFunction(string functionName, int index)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC(functionName, RpcTarget.AllBufferedViaServer, index);
        }
    }


    public void SetCanMouseAction(bool request)
    {
        GameManager.TrollerData.canSetTrap = request;
    }


    public void SetTrapListUpdate(bool isAdd)
    {
        if (isAdd)
        {
            GameManager.TrollerData._setTrapPlatforms.Add(this);
        }
        else
        {
            GameManager.TrollerData._setTrapPlatforms.Remove(this);
        }
        GameManager.TrollerData.leftTrapUI.SetCurrentTrapCountInfo();
    }

}