using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;           //NICKNAME�� �޾� ����Ǵ� text
    [SerializeField] TMP_Text playerReady;          //������ ���� ���θ� �����ִ� text
    [SerializeField] Image image;                   //���� ������ Entry���� ������ �� �� �� �ֵ��� �ϱ����� image��ü�� ���� �޾ƿ�
    [SerializeField] Sprite meSprite;               //���� ������ Entry���� ������ �� �� �� �ֵ��� �ϱ����� image��ü�� ���� �޾ƿ�

    //�÷��̾� �̸��� �޾Ƽ� �ؽ�Ʈȭ ���ִ� �Լ�
    public void SetPlayer(Player player)
    {
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "�غ� �Ϸ�!" : "";
    }

    //���� �������ִ� �Լ�
    public void SetPlayerTrollerTeam()
    {
        if(!GameManager.Team.SetTeam(PlayerTeam.Troller))
            GameManager.Team.SwitchTeam(PlayerTeam.Troller);
    }

    public void SetPlayerClimberTeam()
    {
        if(!GameManager.Team.SetTeam(PlayerTeam.Climber))
            GameManager.Team.SwitchTeam(PlayerTeam.Climber);
    }

    //�÷��̾� ������Ƽ�� ����Ǹ� ����Ǵ� �Լ�
    public void ChangeCustomProperty(PhotonHashtable property)
    {
        if (property.TryGetValue(CustomProperty.READY, out object value))
        {
            bool ready = (bool)value;
            playerReady.text = ready ? "�غ� �Ϸ�!" : "";
        }
    }

    //���� ������ ���� �ʱ�ȭ ���ִ� �Լ�
    public void LeaveRoom()
    {
        GameManager.Team.LeaveTeam();
    }
    
    //���� ������ playerentry�� ����Ȯ���ϰ� �˸��� ���� �Լ�
    public void Sprite()
    {
        image.sprite = meSprite;
    }
}