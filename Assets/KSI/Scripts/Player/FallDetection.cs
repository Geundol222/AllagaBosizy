using UnityEngine;

public class FallDetection : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private int fallPoint; // ������ ����
    [SerializeField] private int fallDegree = 10; // ������ ����

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
			animator.SetBool("IsFalling", true);
		}
		else
		{
			Debug.Log("�÷��̾ �Ͼ���ϴ�.!");
			animator.SetBool("IsFalling", false);
		}
	}
}
