using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIDisappear : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(ActiveFalseRoutine());
		Invoke("ActiveFalse", 5.0f);
	}

	// ��Ȱ��ȭ �ڷ�ƾ
	IEnumerator ActiveFalseRoutine()
	{
		while(true)
		{
			yield return null;
		}
	}

	private void ActiveFalse()
	{ 
		this.gameObject.SetActive(false);
	}
}