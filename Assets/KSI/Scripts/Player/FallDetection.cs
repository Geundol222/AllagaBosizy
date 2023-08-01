using System.Collections.Generic;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] private List<float> fallHeights; // ������ �������� List�� ����

	private Animator animator;
	//private bool isFalling = false;

	private void Start()
	{
		animator = GetComponent<Animator>();
		//isFalling = false;
	}

	private void Update()
	{
	// ���� ��ġ�� ���̸� ������
	float currentHeight = transform.position.y;

	// �������� �ִϸ��̼��� ��� ���� �ƴ϶��
	
		// �������� �������� ���������� üũ�Ͽ� �ִϸ��̼� ���
		foreach (float fallHeight in fallHeights)
		{
			if (currentHeight >= fallHeight)
			{
				animator.SetTrigger("IsFall"); // �ִϸ��̼� Ʈ���� "Fall"�� ȣ���Ͽ� �������� �ִϸ��̼� ���
				break;
			}
		}
	}
	
}