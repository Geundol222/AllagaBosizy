using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class TimerViewTest : MonoBehaviourPunCallbacks
{

	[SerializeField] TMP_Text infoText;
	[SerializeField] float countDownTimer;

	[SerializeField] private float limitTime = 300f; // ���� �ð� 5��
	private float remainLimitTime; // ���� ���� �ð�

	private TMP_Text timerText;

	private void Awake()
	{
		timerText = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LocalPlayer.SetLoad(true);
		}
		else
		{
			infoText.text = "Degug Mode";
			PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public override void OnConnectedToMaster()
	{
		RoomOptions options = new RoomOptions() { IsVisible = false };
		PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
	}

	public override void OnJoinedRoom()
	{
		StartCoroutine(DebugGameSetupDelay());
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log($"Disconnected : {cause}");
	}

	public override void OnLeftRoom()
	{
		Debug.Log("Left Room");
	}

	/* ����(MasterClient) ���� �°�(Migration) */
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.Log("���� �ٲ�");
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
	{
		if (changedProps.ContainsKey("Load"))
		{
			// ��� �÷��̾� �ε� �Ϸ�
			if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
			{
				// ���� ����
				Debug.Log($"All Player Loaded");
				infoText.text = $"All Player Loaded";

				// ���常 ���� �ð� ����
				if (PhotonNetwork.IsMasterClient)
					PhotonNetwork.CurrentRoom.SetLoadTime(PhotonNetwork.ServerTimestamp);
			}
			// �Ϻ� �÷��̾� �ε� �Ϸ�
			else
			{
				// �ٸ� �÷��̾� �ε� �� �� ���� ���
				Debug.Log($"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}");
				infoText.text = $"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}";
			}
		}
	}

	public override void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("LoadTime"))
		{
			StartCoroutine(GameStartTimer());
		}
	}

	IEnumerator GameStartTimer()
	{
		int loadTime = PhotonNetwork.CurrentRoom.GetLoadTime();
		while (countDownTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
		{
			int remainTime = (int)(countDownTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);
			infoText.text = $"All Player Loaded,\nStart CountDown : {remainTime + 1}";
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("Game Start!");
		infoText.text = "Game Start!";
		GameStart();

		yield return new WaitForSeconds(1f);
		infoText.text = "";
	}

	private void DisplayTimer(float second)
	{
		remainLimitTime = second;
		StartCoroutine(UpdateTimerRoutine());
	}

	// Ÿ�̸� �ڷ�ƾ
	private IEnumerator UpdateTimerRoutine()
	{
		while (remainLimitTime >= 0)
		{
			int minutes = Mathf.FloorToInt(remainLimitTime / 60);
			int seconds = Mathf.FloorToInt(remainLimitTime % 60);
			timerText.text = $"{minutes:00} : {seconds:00}";

			remainLimitTime--;
			yield return new WaitForSeconds(1f);
		}

		TimeOut();
	}

	private void TimeOut()
	{
		timerText.text = "TIME OUT";
		timerText.color = Color.red;
	}

	private void GameStart()
	{
		Debug.Log("Normal Game Mode");
		// TODO : GameStart
	}

	// 1�� ������ �ڷ�ƾ
	IEnumerator DebugGameSetupDelay()
	{
		// �������� ���� �ð� 1�� ��ٷ��ֱ�
		yield return new WaitForSeconds(1f);
		DebugGameStart();
	}

	private void DebugGameStart()
	{
		Debug.Log("Debug Game Mode. IsMasterClient : " + PhotonNetwork.IsMasterClient);

		StartCoroutine(UpdateTimerRoutine());
	}

	private int PlayerLoadCount()
	{
		int loadCount = 0;
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			if (player.GetLoad())
				loadCount++;
		}
		return loadCount;
	}
}
