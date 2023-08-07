using Photon.Pun;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScoreCountView : MonoBehaviour
{
	[Header("Transform")]
	[SerializeField] private Transform player; // �÷��̾� ��ġ
	[SerializeField] private Transform startPoint; // ���� ����
	[SerializeField] private Transform endPoint; // ������ ����

	[Header("ScoreUI")]
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private Slider scoreSlider;

	PlayerController playerController;

	private void Awake()
	{
		scoreText = GetComponentInChildren<TextMeshProUGUI>();

		StartCoroutine(NetworkConnectCheckRoutine());
	}

	IEnumerator NetworkConnectCheckRoutine()
	{
		yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });

		scoreSlider.enabled = true;

		if (PhotonNetwork.IsConnected)
			yield break;
	}

	private void Start()
	{
		//scoreSlider.maxValue = 100;
		//scoreSlider.minValue = 0;

		//scoreSlider.value = 0;
	}

	private void Update()
	{
		ScoreCalculate();
	}

	private void ScoreCalculate()
	{
		// ���� ������ ������ ���� ������ y �Ÿ� ���
		float totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);

		// ���� ������ �÷��̾� ������ y �Ÿ� ���
		float playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);

		// ���� �������� �÷��̾������ y �Ÿ� ����� ���
		float percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);

		// ����� ���� ������ ��ȯ
		int score = Mathf.RoundToInt(percentage);

		// ����� ���� �ؽ�Ʈ�� ǥ��
		scoreText.text = score.ToString() + "%";

		// ����� ���� �����̴��� ����
		scoreSlider.value = score;
	}


}