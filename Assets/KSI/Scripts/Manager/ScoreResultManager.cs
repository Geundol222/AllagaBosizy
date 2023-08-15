using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResultManager :MonoBehaviourPunCallbacks
{
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private TMP_Text resultText;
	private int teamAScore = 0;
	private int teamBScore = 0;
	private int currentRound = 1;
	private int myScore;

	private void Start()
	{
		teamAScore = 0;
		teamBScore = 0;
		currentRound = 1;

		UpdateScoreText();
	}

	// �÷��̾��� ������ ������Ʈ�ϰ� ���и� Ȯ��
	[System.Obsolete]
	private void UpdateScore()
	{
		myScore = (int)photonView.Owner.CustomProperties["ScoreRound" + currentRound.ToString()]; // ���� ������ �÷��̾��� ���ھ�

		if (photonView.IsMine)
		{
			string myTeam = (string)photonView.Owner.CustomProperties["Team"];
			if (myTeam == "blue")
			{
				teamAScore += myScore;
			}
			else if (myTeam == "red")
			{
				teamBScore += myScore;
			}
		}

		UpdateScoreText();

		if (PhotonNetwork.IsMasterClient)
		{
			// ������ Ŭ���̾�Ʈ���� ���� ���� �� ���и� �����ϰ� ����� ��� �÷��̾�� �˸�
			if (currentRound == 2 && teamAScore + teamBScore >= 2) // �� ���� ���� ���� ����
			{
				if (teamAScore > teamBScore)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Team A");
				}
				else if (teamBScore > teamAScore)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Team B");
				}
				else
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Draw");
				}
			}
		}
	}

	// ���и� �˸��� RPC �Լ�
	[PunRPC]
	private void DeclareWinner(string winner)
	{
		resultText.text = winner + " wins!"; // ���� ���� UI�� ǥ��
		Debug.Log("Winner: " + winner);
	}

	private void UpdateScoreText()
	{
		scoreText.text = "Team A: " + teamAScore + " | Team B: " + teamBScore;
		Debug.Log("Team A: " + teamAScore + " | Team B: " + teamBScore);
	}
}
