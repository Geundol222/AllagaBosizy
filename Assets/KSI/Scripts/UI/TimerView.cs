using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviourPunCallbacks
{
	[SerializeField] private float limitTime = 300f; // ���� �ð� 5��
	private float remainLimitTime; // ���� ���� �ð�

	private int time = 0;
	private TMP_Text timerText;
	public PhotonView pv;


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
		if (PhotonNetwork.IsMasterClient)
		{
			remainLimitTime = second;
			StartCoroutine(UpdateTimerRoutine());
		}	
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
		TimeOut();

		pv.RPC("ShowTimer", RpcTarget.All, time); // 1�ʸ��� �� ��ο��� ����

		yield return new WaitForSeconds(1);
		StartCoroutine(UpdateTimerRoutine());
	}

	[PunRPC]
	private void ShowTimer(int number)
	{
		timerText.text = number.ToString(); //Ÿ�̸� ����
	}

	private void TimeOut()
	{
		timerText.text = "TIME OUT";
		timerText.color = Color.red;
	}	
}