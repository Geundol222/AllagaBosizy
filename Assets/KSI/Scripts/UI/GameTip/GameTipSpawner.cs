using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTipSpawner : MonoBehaviour
{
    [SerializeField] private List<GameTipData> gameTipDatas;
    [SerializeField] private TextMeshProUGUI gameTipText;

	private int randomIndex;
	public GameTipData randomTip;

	private void Awake()
	{
		gameTipText = GetComponent<TextMeshProUGUI>();		
	}

	private void Start()
	{
		StartCoroutine(GameTipRandomRoutine());
	}

	private IEnumerator GameTipRandomRoutine()
	{
		while (true)
		{
			randomIndex = UnityEngine.Random.Range(0, gameTipDatas.Count);
			randomTip = gameTipDatas[randomIndex];

			gameTipText.text = "���� �� ��ȣ : " + randomTip.GameTipNumber + "\n" + "���� : " + randomTip.GameTipDescription;

			yield return new WaitForSeconds(1f);
		}
	}
}
