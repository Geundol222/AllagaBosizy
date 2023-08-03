using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCountView : MonoBehaviour
{
	[Header("Transform")]
	[SerializeField] private Transform player; // �÷��̾� ��ġ
	[SerializeField] private Transform startPoint; // ���� ����
	[SerializeField] private Transform endPoint; // ������ ����

	[Header("ScoreUI")]
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private Slider scoreSlider;

	private void Awake()
	{
		scoreText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		scoreSlider.maxValue = 100;
		scoreSlider.minValue = 0;

		scoreSlider.value = 0;
	}

	private void Update()
	{
		// ���� ������ ������ ���� ������ y �Ÿ� ���
		float totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);

		// ���� ������ �÷��̾� ������ y �Ÿ� ���
		float playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);

		// ���� �������� �÷��̾������ y �Ÿ� ����� ���
		float percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);

		// ����� ���� ������ ��ȯ�Ͽ� �ؽ�Ʈ�� ǥ��
		int score = Mathf.RoundToInt(percentage);
		scoreText.text = score.ToString() + "%";
		// ����� ���� �����̴��� ����
		scoreSlider.value = score;
	}
}