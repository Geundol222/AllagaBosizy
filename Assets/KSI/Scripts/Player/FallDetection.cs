using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private int fallPoint; // ������ ����
    private int fallDegree = 5; // ������ ����

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		fallPoint = (int)rigidbody.position.y; 
	}

	private void Update()
	{
		int currentPoint = (int)rigidbody.position.y;
		int heightDifference = fallPoint - currentPoint;

		if (currentPoint > fallPoint)
		{
			fallPoint = currentPoint;
		}

		if (heightDifference >= fallDegree) 
		{
			Debug.Log("�÷��̾ ���������ϴ�!");

			// animator.SetBool("isFalling", true);
		}
		else
		{
			animator.SetBool("isFalling", false);
		}
	}
}
