using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField chatInputField;
    [SerializeField] TMP_Text chatMessage;
    [SerializeField] RectTransform chatObjectParent;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] BaseEventData eventdata;
    public PhotonView PV;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnEnter();
        }
    }

    //������ ä��â�� ���� �ؽ�Ʈ�� �� ��ü �ο����� ChatRPC�Լ��� �����ϵ��� ���ִ� �Լ�
    private void OnEnter()
    {
        if (chatInputField.text == "")
        {
            chatInputField.ActivateInputField();
            return;
        }
        string mes = chatInputField.text.Trim();
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + mes, PhotonNetwork.LocalPlayer);
        chatInputField.text = "";
        chatInputField.ActivateInputField();
    }

    //���� ������ ä�÷α׸� ����� �Լ�
    public void OutRoom()
    {
        for (int i = 0; i < chatObjectParent.childCount; i++)
        {
            Destroy(chatObjectParent.GetChild(i).gameObject);
        }
    }

    //�濡 �ִ� ���¿��� �ٸ� ������ ���� �� ����Ǵ� �Լ�
    public void InOutRPC(string chat)
    {
        TMP_Text text;
        text = Instantiate(chatMessage, chatObjectParent.transform);
        text.text = chat;
        text.color = Color.yellow;
        scrollbar.value = 0;
    }

    //�濡 �ִ� �ο� ��ü���� ä��â�� �ִ� text�� ä��â �α׿� text�� �־��ִ� �Լ�
    [PunRPC]
    void ChatRPC(string chat, Player player)
    {
        TMP_Text text;
        text = Instantiate(chatMessage, chatObjectParent.transform);
        text.text = chat;
        if (player.GetPlayerTeam() == PlayerTeam.Troller)
        {
            text.color = Color.blue;
        }
        else if (player.GetPlayerTeam() == PlayerTeam.Climber)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.black;
        }
        scrollbar.value = 0;
    }
}
