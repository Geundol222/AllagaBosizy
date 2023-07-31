using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
	[SerializeField] private float limitTime = 300f; // ���� �ð� 5��
	private float remainLimitTime; // ���� ���� �ð�

	private TMP_Text timerText;
	
	private void Awake()
	{
		timerText = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		DisplayTimer(limitTime);
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
		TimeOver();
	}

	private void TimeOver()
	{
		Debug.Log("TimeOver");
		// TODO : Ÿ�ӿ��� UI �߰�???
	}	
}