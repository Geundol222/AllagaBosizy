using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public enum PlayerTeam { None, Troller, Climber }

public enum Climber { None, Goblin, Ghost, Boy, Girl }

public class TeamManager : MonoBehaviourPun
{
    private int climberCount;               //����� �ο����� ���� ���� ����
    private int trollerCount;               //������ �ο����� ���� ���� ����

    private void Awake()
    {
        photonView.ViewID = 999;
    }

    //������ ���� �˷��ִ� �Լ�
    public PlayerTeam GetTeam()
    {
        return PhotonNetwork.LocalPlayer.GetPlayerTeam();
    }

    //������ ���� �ʱ�ȭ ���ִ� �Լ�
    public void LeaveTeam()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("Team");
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
    }

    //������ ���� �������ִ� �Լ�
    public bool SetTeam(PlayerTeam team)
    {
        //������ ���� ���� ���� ������ �����ϵ��� ����
        if (PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.None)
        {
            if (PhotonNetwork.LocalPlayer.JoinTeam((byte)team))
            {
                PhotonNetwork.LocalPlayer.SetPlayerTeam(team);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    //������ ���� �ٲ��ִ� �Լ�
    public bool SwitchTeam(PlayerTeam team)
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("Team");

        if (PhotonNetwork.LocalPlayer.SwitchTeam((byte)team))
        {
            PhotonNetwork.LocalPlayer.SetPlayerTeam(team);
            return true;
        }
        else
        {
            return false;
        }
    }

    //
    public void TeamSplit()
    {
        climberCount = 0;
        trollerCount = 0;

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.GetPlayerTeam() == PlayerTeam.Climber)
                {
                    photonView.RPC("SetClimber", RpcTarget.AllBuffered, player, climberCount++);
                }
                else if ((player.GetPlayerTeam() == PlayerTeam.Troller))
                {
                    photonView.RPC("SetTroller", RpcTarget.AllBuffered, player, trollerCount++);
                }
                else
                    player.SetClimber(Climber.None);
            }
        }
    }

    //������ ������ �ִ� �Լ�
    [PunRPC]
    public void SetClimber(Player player, int count)
    {
        if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (count == 0)
            {
                player.SetClimber(Climber.Boy);
            }
            else if (count == 1)
            {
                player.SetClimber(Climber.Girl);
            }
            else
                return;
        }
    }

    [PunRPC]
    public void SetTroller(Player player, int count)
    {
        if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (count == 0)
            {
                player.SetClimber(Climber.Goblin);
            }
            else if (count == 1)
            {
                player.SetClimber(Climber.Ghost);
            }
            else
                return;
        }
    }
}