using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class ScoreCountView : MonoBehaviour
{
	[SerializeField] private DataManager dataManager;

	private TMP_Text scoreText;

	private void Awake()
	{
		scoreText = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		// ���� ���� �� 0������ �ʱ�ȭ��
		DisplayScore(0);	
	}

	private void OnEnable()
	{
		dataManager.OnCurrentScoreChanged += DisplayScore;
	}

	private void OnDisable()
	{
		dataManager.OnCurrentScoreChanged -= DisplayScore;
	}

	public void DisplayScore(int score)
    {
		scoreText.text = "SCORE : "  + score.ToString();
	}   
}