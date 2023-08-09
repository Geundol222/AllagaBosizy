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
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] TeamManager teamManager;
    [SerializeField] Image image;
    [SerializeField] Sprite meSprite;
    
    public void SetPlayer(Player player)
    {
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "�غ� �Ϸ�!" : "";
    }

    public void SetPlayerTrollerTeam()
    {
        teamManager.SetTeam(PlayerTeam.Troller);
    }

    public void SetPlayerClimberTeam()
    {
        teamManager.SetTeam(PlayerTeam.Climber);
    }

    public void ChangeCustomProperty(PhotonHashtable property)
    {
        if (property.TryGetValue(CustomProperty.READY, out object value))
        {
            bool ready = (bool)value;
            playerReady.text = ready ? "�غ� �Ϸ�!" : "";
        }
    }

    public void LeaveRoom()
    {
        teamManager.LeaveTeam();
    }

    public string GetTeam()
    {
        return PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;
    }
    
    public void Sprite()
    {
        image.sprite = meSprite;
    }
}