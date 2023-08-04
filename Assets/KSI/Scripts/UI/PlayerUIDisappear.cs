using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIDisappear : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(DisappearRoutine());
		Invoke("ActiveFalse", 5.0f);
	}

	// ��Ȱ��ȭ �ڷ�ƾ
	IEnumerator DisappearRoutine()
	{
		while(true)
		{
			yield return null;

			Debug.Log("Active");
		}
	}

	private void ActiveFalse()
	{ 
		this.gameObject.SetActive(false);
	}
}
