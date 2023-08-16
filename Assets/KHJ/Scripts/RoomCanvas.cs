using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomCanvas : MonoBehaviour
{
    public Dictionary<int, PlayerEntry> playerDictionary;
    public Dictionary<int, PlayerEntry> aTeamDictionary;
    public Dictionary<int, PlayerEntry> bTeamDictionary;
    [SerializeField] RectTransform playerContent1;
    [SerializeField] RectTransform playerContent2;
    [SerializeField] Button startButton;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] LogImage logImage;
    [SerializeField] LobbyManager lobbyManager;
    PhotonView PV;
    PlayerEntry entry;
    Animator anim;
    bool isStart;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
        aTeamDictionary = new Dictionary<int, PlayerEntry>();
        bTeamDictionary = new Dictionary<int, PlayerEntry>();
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    //�濡 ó�� ������ �÷��̾� ����Ʈ�� �޾� ���� �������
    private void OnEnable()
    {
        isStart = false;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                //�� ���� ��� ������ �� �ִ��� �Ǻ��� ������ ������ ��
                if (player.GetPlayerTeam() == PlayerTeam.Troller)
                {
                    entry = Instantiate(playerEntryPrefab, playerContent1);
                    entry.SetPlayer(player);
                    aTeamDictionary.Add(player.ActorNumber, entry);
                }
                else
                {
                    entry = Instantiate(playerEntryPrefab, playerContent2);
                    entry.SetPlayer(player);
                    bTeamDictionary.Add(player.ActorNumber, entry);
                }
            }
            else
            {
                //ateamdictionary�� 2���̻��ϰ�� ���ö� b������ ������ ����
                if (aTeamDictionary.Count < 2)
                {
                    entry = Instantiate(playerEntryPrefab, playerContent1);
                    entry.SetPlayer(player);
                    aTeamDictionary.Add(player.ActorNumber, entry);
                    entry.SetPlayerTrollerTeam();
                }
                else
                {
                    entry = Instantiate(playerEntryPrefab, playerContent2);
                    entry.SetPlayer(player);
                    bTeamDictionary.Add(player.ActorNumber, entry);
                    entry.SetPlayerClimberTeam();
                }
            }
            if (player == PhotonNetwork.LocalPlayer)
            {
                entry?.Sprite();
            }
            playerDictionary.Add(player.ActorNumber, entry);
            PhotonNetwork.LocalPlayer.SetReady(false);
            PhotonNetwork.LocalPlayer.SetLoad(false);
            AllPlayerReadyCheck();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    //�˹����� �������� ������ �Լ�
    private void OnDisable()
    {
        foreach (int actorNumber in playerDictionary.Keys)
        {
            Destroy(playerDictionary[actorNumber].gameObject);
        }
        playerDictionary.Clear();
        aTeamDictionary.Clear();
        bTeamDictionary.Clear();
    }

    //�ٸ� ������ ���� �� �� ������ �ش��ϴ� playerentry�� ������.
    public void PlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        if(!aTeamDictionary.Remove(otherPlayer.ActorNumber))
            bTeamDictionary.Remove(otherPlayer.ActorNumber);
        AllPlayerReadyCheck();
    }

    //������ properties�� ����ɋ����� ����Ǵ� �Լ�
    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);
        AllPlayerReadyCheck();
    }

    //������ �ٲ���� �� ����Ǵ� �Լ�
    public void MasterClientSwitched(Player newMasterClient)
    {
        AllPlayerReadyCheck();
    }

    //������ ���۵Ǹ� ����Ǵ� �Լ�
    public void StartGame()
    {
        //�̹� �����Լ��� ����Ǿ��µ� �ٽ� ���� �� �ִ� ��츦 ����
        if (isStart)
        {
            return;
        }
        isStart = true;
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.SetCurrentRound(Round.NONE);

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        GameManager.Scene.LoadScene(Scene.LOADING);
    }

    //���� ������ ����Ǵ� �Լ�. animation�� ���� �ڷ�ƾ���� ����
    public void LeaveRoom()
    {
        StartCoroutine(LeaveRoomRoutine());
    }

    //�÷��̾� ���� ��ư�� ������ ���� ���θ� ���� ���°� �ٲ�� �ϴ� �Լ�
    public void PlayerReady()
    {
        if (PhotonNetwork.LocalPlayer.GetReady())
        {
            PhotonNetwork.LocalPlayer.SetReady(false);
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetReady(true);
        }
    }

    //�÷��̾ ��� ���� �Ǹ� ���忡�� start��ư�� Ȱ��ȭ �����ִ� �Լ�
    private void AllPlayerReadyCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
            return;
        }

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
        }
        //���� 2��2�θ� ���� �Ǳ� ������ 2��2�϶��� üũ�ϵ��� ����
        if (readyCount == PhotonNetwork.PlayerList.Length && aTeamDictionary.Count == 2 && bTeamDictionary.Count == 2)
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }

    //b������ a������ �ٲ��ִ� �Լ�
    public void SwitchTeamA()
    {
        if (PhotonNetwork.LocalPlayer.GetReady() || PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Troller)
        {
            return;
        }
        //���ο� ��ü���� �Լ��� ������� ���� ����ȭ ��Ű���� ��.
        PV.RPC("PlayerRoomUpdate", RpcTarget.All, PhotonNetwork.LocalPlayer, true, true);
    }

    //b������ a������ �ٲ��ִ� �Լ�
    public void SwitchTeamB()
    {
        if (PhotonNetwork.LocalPlayer.GetReady() || PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Climber)
        {
            return;
        }
        PV.RPC("PlayerRoomUpdate", RpcTarget.All, PhotonNetwork.LocalPlayer, true, false);
    }

    
    IEnumerator LeaveRoomRoutine()
    {
        PlayerEntry playerEntry = playerDictionary[PhotonNetwork.LocalPlayer.ActorNumber];
        playerEntry.LeaveRoom();
        anim.SetTrigger("OutRoom");
        yield return new WaitForSeconds(1);
        PhotonNetwork.LeaveRoom();
        yield break;
    }

    //�������Ʈ�� ���ִ� �Լ�, ������ �ƴ� ������ �����ų�, ������ ������ ������ ���� �ٲٸ� bool������ ������ ���� ���� �����ϰų� ���� �ٲٴ� ���� �����Ѵ�.
    [PunRPC]
    public void PlayerRoomUpdate(Player newPlayer, bool isSwitch, bool isAteamSwitch)
    {
        //����ġ�� Ʈ�簪�̸� ���� �ٲ��ִ� �Լ���, �ƴϸ� ���� ������ ������ �־��ִ� �Լ��� �����ϵ��� ����
        if (!isSwitch)
        {
            PlayerEntry entry;
            if (aTeamDictionary.Count < 2)
            {
                entry = Instantiate(playerEntryPrefab, playerContent1);
                entry.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry);
                aTeamDictionary.Add(newPlayer.ActorNumber, entry);
            }
            else
            {
                entry = Instantiate(playerEntryPrefab, playerContent2);
                entry.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry);
                bTeamDictionary.Add(newPlayer.ActorNumber, entry);
            }
            AllPlayerReadyCheck();
        }
        else
        {
            Destroy(playerDictionary[newPlayer.ActorNumber].gameObject);
            playerDictionary.Remove(newPlayer.ActorNumber);
            if (!aTeamDictionary.Remove(newPlayer.ActorNumber))
                bTeamDictionary.Remove(newPlayer.ActorNumber);
            if (isAteamSwitch)
            {
                PlayerEntry entry2;
                entry2 = Instantiate(playerEntryPrefab, playerContent1);
                entry2.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry2);
                aTeamDictionary.Add(newPlayer.ActorNumber, entry2);
                if (newPlayer == PhotonNetwork.LocalPlayer)
                {
                    entry.SetPlayerTrollerTeam();
                    entry2?.Sprite();
                }
            }
            else
            {
                PlayerEntry entry2;
                entry2 = Instantiate(playerEntryPrefab, playerContent2);
                entry2.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry2);
                bTeamDictionary.Add(newPlayer.ActorNumber, entry2);
                if (newPlayer == PhotonNetwork.LocalPlayer)
                {
                    entry.SetPlayerClimberTeam();
                    entry2?.Sprite();
                }
            }
            AllPlayerReadyCheck();
        }
    }
}