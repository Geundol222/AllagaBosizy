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

	private void Start()
	{
		teamAScore = 0;
		teamBScore = 0;

		UpdateScore();
	}

	// �÷��̾��� ������ ������Ʈ�ϰ� ���и� Ȯ��
	private void UpdateScore()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			foreach (Player player in PhotonNetwork.PlayerList)
			{
				if (player.GetPlayerTeam() == PlayerTeam.Troller)
				{
					teamBScore += player.GetScore();
				}
				else if (player.GetPlayerTeam() == PlayerTeam.Climber)
				{
					teamAScore += player.GetScore();
				}
			}

			photonView.RPC("DeclareWinner", RpcTarget.All, (teamAScore / 2), (teamBScore / 2));
		}
	}

	// ���и� �˸��� RPC �Լ�
	[PunRPC]
	private void DeclareWinner(int aScore, int bScore)
	{
		if (aScore > bScore)
		{
			resultTextTeamA.text = "WIN !";
			resultTextTeamB.text = "DEFEAT !";
		}
		else if (bScore > aScore)
		{
			resultTextTeamB.text = "WIN !";
			resultTextTeamA.text = "DEFEAT !";
		}

		UpdateScoreText(aScore, bScore);
	}

	private void UpdateScoreText(int aScore, int bScore)
	{
		teamAScoreText.text = $"Team A : {aScore}";
		teamBScoreText.text = $"Team B : {bScore}";
	}
}
