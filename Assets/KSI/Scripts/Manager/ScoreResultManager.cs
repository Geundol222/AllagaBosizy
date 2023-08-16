using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ScoreResultManager : MonoBehaviourPunCallbacks
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
	private void UpdateScore()
	{
		
		if (currentRound == 1)
		{
			myScore = teamAScore; // ù ��° ��� ���� ������ ���
		}
		else if (currentRound == 2)
		{
			myScore = teamBScore; // �� ��° ��� ���� ������ ���
		}

		if (photonView.IsMine)
		{
			string myTeam = ((PlayerTeam)PhotonNetwork.LocalPlayer.GetPlayerTeam()).ToString();
			if (myTeam == "TeamA")
			{
				teamAScore += myScore;
			}
			else if (myTeam == "TeamB")
			{
				teamBScore += myScore;
			}
		}

		UpdateScoreText();

		if (PhotonNetwork.IsMasterClient)
		{
			// ������ Ŭ���̾�Ʈ���� ���� ���� �� ���и� �����ϰ� ����� ��� �÷��̾�� �˸�
			// �� ���� ���� ���� ����
			if (currentRound == 2)
			{
				int bestScoreTeamA = GetBestScoreByTeam(PlayerTeam.Troller);
				int bestScoreTeamB = GetBestScoreByTeam(PlayerTeam.Climber);

				// �� A�� �� B�� ���� ���� ���Ͽ� ���� ����
				if (bestScoreTeamA > bestScoreTeamB)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "TeamA");
				}
				else if (bestScoreTeamB > bestScoreTeamA)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "TeamB");
				}
				else
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Draw");
				}
			}
		}
	}

	private int GetBestScoreByTeam(PlayerTeam team)
	{
		int bestScore = 0;

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			if (player.GetPlayerTeam() == team)
			{
				int playerScore = player.GetScore();
				if (playerScore > bestScore)
				{
					bestScore = playerScore;
				}
			}
		}
		return bestScore;
	}

	// ���и� �˸��� RPC �Լ�
	[PunRPC]
	private void DeclareWinner(string winner)
	{
		if ((string)photonView.Owner.CustomProperties["Team"] == "TeamA")
		{
			resultTextTeamA.text = "WIN !";
			resultTextTeamB.text = "DEFEAT !";
		}
		else if ((string)photonView.Owner.CustomProperties["Team"] == "TeamB")
		{
			resultTextTeamA.text = "DEFEAT !";
			resultTextTeamB.text = "WIN !";
		}
	}

	private void UpdateScoreText()
	{
		teamAScoreText.text = "Team A : " + teamAScore;
		teamBScoreText.text = "Team B : " + teamBScore;
	}
}
