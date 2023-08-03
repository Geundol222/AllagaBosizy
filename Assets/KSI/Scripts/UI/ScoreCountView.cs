using TMPro;
using UnityEngine;

public class ScoreCountView : MonoBehaviour
{
	[SerializeField] private Transform player; // �÷��̾� ��ġ
	[SerializeField] private Transform startPoint; // ���� ����
	[SerializeField] private Transform endPoint; // ������ ����

	private TMP_Text scoreText;

	private void Awake()
	{
		scoreText = GetComponent<TMP_Text>();
	}

	void Update()
	{
		// ���� ������ ������ ���� ������ y �Ÿ� ���
		float totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);

		// ���� ������ �÷��̾� ������ y �Ÿ� ���
		float playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);

		// ���� �������� �÷��̾������ y �Ÿ� ����� ���
		float percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);

		// ����� ���� ������ ��ȯ�Ͽ� ǥ��
		int score = Mathf.RoundToInt(percentage);
		scoreText.text = "SCORE : " + score.ToString() + " %";
	}
}