using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

//������ �Լ����� ����Ǹ� �׿����� ���� ���¸� �ٲٱ� ���� ���۵� ��ũ��Ʈ
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Room }

    [SerializeField] LoginCanvas loginCanvas;
    [SerializeField] MenuCanvas menuCanvas;
    [SerializeField] RoomCanvas roomCanvas;
    [SerializeField] ChatCanvas chatCanvas;

    //ó�� ������ �� ���¿� ���� �ٸ� canvas�� �������� ����
    public void Start()
    {
        GameManager.Sound.PlaySound("MainLobbyRoom/bgm", Audio.BGM);
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                OnJoinedRoom();
                return;
            }
            else if (PhotonNetwork.InLobby)
            {
                OnJoinedLobby();
                return;
            }
            OnConnectedToMaster();
        }
        else
            OnDisconnected(DisconnectCause.None);
    }

    //������ ���� �� �� ����Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        SetActivePanel(Panel.Menu);
    }

    //�������� ������ ����Ǵ� �Լ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    //���� ����� ���� ���������� ����Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
    }

    //�뿡 ���� ���� �������� �� ����Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
    }

    //�뿡 �� �� ����Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        SetActivePanel(Panel.Room);
        chatCanvas.OutRoom();
        PhotonNetwork.LocalPlayer.SetReady(false);
    }
    
    //�α��εǾ��� �� ����Ǵ� �Լ�
    public void OnLoginCanvas()
    {
        SetActivePanel(Panel.Login);
    }

    //�뿡�� ���� �� ����Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.JoinLobby();
    }

    //�뿡 ���� �� �ٸ� ������ ������ ����Ǵ� �Լ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomCanvas.PlayerRoomUpdate(newPlayer, false, false);
        chatCanvas.InOutRPC(newPlayer.NickName + "���� �����ϼ̽��ϴ�.");
    }

    //�뿡 ���� �� �ٸ� ������ ������ ����Ǵ� �Լ�
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomCanvas.PlayerLeftRoom(otherPlayer);
        chatCanvas.InOutRPC(otherPlayer.NickName + "���� �����ϼ̽��ϴ�.");
    }

    //���� Ȥ�� �׿ܿ� �÷��̾��� properties�� ����Ǹ� ����Ǵ� �Լ�
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        roomCanvas.PlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    //������ �ٲ� �� ����Ǵ� �Լ�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomCanvas.MasterClientSwitched(newMasterClient);
    }

    //�κ� �� �� ����Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        SetActivePanel(Panel.Menu);
    }

    //�κ񿡼� ���� �� ����Ǵ� �Լ�, �α׾ƿ� �� ���ۿ� ������� �ʴ´�.
    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.Login);
    }

    //�κ񿡼� �� ����Ʈ�� ������Ʈ �� ������ ����Ǵ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        menuCanvas.UpdateRoomList(roomList);
    }

    //���� ���¿� ���� �˹����� ��������ִ� �Լ�
    private void SetActivePanel(Panel panel)
    {
        if (loginCanvas != null) loginCanvas.gameObject.SetActive(panel == Panel.Login);
        if (menuCanvas != null) menuCanvas.gameObject.SetActive(panel == Panel.Menu);
        if (roomCanvas != null) roomCanvas.gameObject.SetActive(panel == Panel.Room);
    }

    public void PlayUIButtonClickSound()
    {
        GameManager.Sound.PlaySound("MainLobbyRoom/MouseClick");
    }

}