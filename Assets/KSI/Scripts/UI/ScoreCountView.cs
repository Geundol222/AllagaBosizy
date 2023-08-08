using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCountView : MonoBehaviourPunCallbacks, IPunObservable
{
	[Header("Transform")]
	[SerializeField] private Transform startPoint; // ���� ����
	[SerializeField] private Transform endPoint; // ������ ����

	[Header("ScoreUI")]
	[SerializeField] private Slider scoreSlider;

	private Transform player;
	private float totalYDistance; //  ���� ������ ������ ���� ������ y �Ÿ�
	private float playerYDistance; // ���� ������ �÷��̾� ������ y �Ÿ�
	private float percentage; // ���� �������� �÷��̾������ y �Ÿ� �����
	private int score; 
	private int bestScore; // �ְ��� �ӽ� ����

	bool gameStart = false;

	private void Awake()
	{
		StartCoroutine(NetworkConnectCheckRoutine());
	}

	IEnumerator NetworkConnectCheckRoutine()
	{
		yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });

		scoreSlider.enabled = true;

		if (PhotonNetwork.IsConnected)
		{
			if (photonView.IsMine)
			{
				if (gameObject.name == "ScoreSliderBoy")
				{
					if (GameObject.Find("PlayerBoy(Clone)"))
					{
						player = GameObject.Find("PlayerBoy(Clone)").transform;
					}
				}
				else if (gameObject.name == "ScoreSliderGirl")
				{
					if (GameObject.Find("PlayerGirl(Clone)"))
					{
						player = GameObject.Find("PlayerGirl(Clone)").transform;
					}
				}				
			}

			yield break;
		}
	}

	private void Update()
	{
		if (player != null)
		{
			ScoreCalculate();
		}			
		else 
		{
			if (GameObject.Find("PlayerBoy(Clone)"))
			{
				player = GameObject.Find("PlayerBoy(Clone)").transform;
			}
			else if (GameObject.Find("PlayerGirl(Clone)"))
			{
				player = GameObject.Find("PlayerGirl(Clone)").transform;
			}
		}
	}

	private void ScoreCalculate()
	{
		
		totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);

		playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);

		percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);

		// ����� ���� ������ ��ȯ
		score = Mathf.RoundToInt(percentage);

		// ���� ������ �ְ� ������ �ʰ��ϸ� �ְ� ������ ������Ʈ�ϰ� PlayerPrefs�� ����
		if (score > bestScore)
		{
			bestScore = score;
			PlayerPrefs.SetInt("BestScore", bestScore);
			
			scoreSlider.value = score;
			Debug.Log("New Best Score: " + bestScore);
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
			this.scoreSlider.value = (float)stream.ReceiveNext();
		}
	}
}