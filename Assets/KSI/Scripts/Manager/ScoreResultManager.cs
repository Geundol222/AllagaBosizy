using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResultManager :MonoBehaviourPunCallbacks
{
	[SerializeField] private TMP_Text teamAScoreText;
	[SerializeField] private TMP_Text teamBScoreText;
	[SerializeField] private TMP_Text resultTextTeamA;
	[SerializeField] private TMP_Text resultTextTeamB;

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
		// ���� ������ �÷��̾��� ���ھ�
		myScore = (int)photonView.Owner.CustomProperties["ScoreRound" + currentRound.ToString()]; 

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
			// �� ���� ���� ���� ����
			if (currentRound == 2 && teamAScore + teamBScore >= 2)
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
		if ((string)photonView.Owner.CustomProperties["Team"] == "blue")
		{
			resultTextTeamA.text = winner;
			resultTextTeamB.text = "�й�!";
		}
		else if ((string)photonView.Owner.CustomProperties["Team"] == "red")
		{
			resultTextTeamA.text = "�й�!";
			resultTextTeamB.text = winner;
		}

		Debug.Log("����: " + winner);
	}

	private void UpdateScoreText()
	{
		teamAScoreText.text = "Team A: " + teamAScore;
		teamBScoreText.text = "Team B: " + teamBScore;
	}
}
