using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviourPunCallbacks //, IPunObservable
{
	[SerializeField] private float limitTime = 300f; // ���� �ð� 5��
	private float remainLimitTime; // ���� ���� �ð�

	private TMP_Text timerText;

	private void Awake()
	{
		timerText = GetComponent<TMP_Text>();

		// Photon Network �ʱ�ȭ
		//PhotonNetwork.ConnectUsingSettings();
	}

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			// ������ Ŭ���̾�Ʈ�� ��� Ÿ�̸Ӹ� �����ϰ� �ٸ� Ŭ���̾�Ʈ�� ����ȭ
			photonView.RPC("CallDisplayTimerRPC", RpcTarget.All, limitTime);
		}
	}

	private void DisplayTimer(float second)
	{
		remainLimitTime = second;
		StartCoroutine(UpdateTimerRoutine());
	}

	[PunRPC]
	public void CallDisplayTimerRPC(float second)
	{
		// ���� PhotonView�� ���� RPC�� ȣ��
		photonView.RPC("DisplayTimer", RpcTarget.All, second);
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
	}

	private void TimeOut()
	{
		timerText.text = "TIME OUT";
		timerText.color = Color.red;
	}

	//public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	//{
	//	if (stream.IsWriting)
	//	{
	//		stream.SendNext(remainLimitTime);
	//	}
	//	else
	//	{
	//		remainLimitTime = (float)stream.ReceiveNext();
	//	}
	//}
}