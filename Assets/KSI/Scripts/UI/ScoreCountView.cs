using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCountView : MonoBehaviourPunCallbacks, IPunObservable
{
	[SerializeField] Slider scoreSlider;
	[SerializeField] private Transform startPoint; // ���� ����
	[SerializeField] private Transform endPoint; // ������ ����

	private Transform player;

	private float totalYDistance; //  ���� ������ ������ ���� ������ y �Ÿ�
	private float playerYDistance; // ���� ������ �÷��̾� ������ y �Ÿ�
	private float percentage; // ���� �������� �÷��̾������ y �Ÿ� �����
	private int score;
	private int bestScore; // �ְ��� �ӽ� ����

	public int BestScore { get { return bestScore; } }

	private void Start()
	{
		StartCoroutine(NetworkConnectCheckRoutine());
		StartCoroutine(PlayerFindRoutine());

		// �ʱ�ȭ �ÿ� bestScore ���� �÷��̾� Properties�� ����
		bestScore = 0;
		if (PhotonNetwork.IsConnectedAndReady)
		{
			PhotonNetwork.LocalPlayer.SetScore(bestScore);
		}
	}

	IEnumerator NetworkConnectCheckRoutine()
	{
		yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });

		if (PhotonNetwork.IsConnected)
		{
			yield break;
		}
	}

	IEnumerator PlayerFindRoutine()
	{
		yield return new WaitUntil(() => { return gameObject.name == "ScoreSliderBoy" ? GameObject.Find("PlayerBoy(Clone)") : GameObject.Find("PlayerGirl(Clone)"); });

		if (gameObject.name == "ScoreSliderBoy")
		{
			player = GameObject.Find("PlayerBoy(Clone)").transform;
		}
		else if (gameObject.name == "ScoreSliderGirl")
		{
			player = GameObject.Find("PlayerGirl(Clone)").transform;
		}

		yield break;
	}

	private void Update()
	{
		if (player != null)
			ScoreCalculate();
	}

	private void ScoreCalculate()
	{
		totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);
		playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);
		percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);
		score = Mathf.RoundToInt(percentage);

		BestScoreUpdateChecker(score);
	}

	private void BestScoreUpdateChecker(int score)
	{
		if (score > bestScore)
		{
			bestScore = score;
			scoreSlider.value = bestScore;
			PhotonNetwork.LocalPlayer.SetScore(bestScore);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(scoreSlider.value);
		}
		else
		{
			scoreSlider.value = (float)stream.ReceiveNext();
		}
	}
}