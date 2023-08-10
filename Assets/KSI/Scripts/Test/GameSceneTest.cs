using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneTest : BaseSceneTest
{
	protected override IEnumerator LoadingRoutine()
	{
		progress = 0.0f;
		Debug.Log("�� ����");
		yield return new WaitForSeconds(1f);

		progress = 0.2f;
		Debug.Log("���� ����");
		yield return new WaitForSeconds(1f);

		progress = 0.4f;
		Debug.Log("����� ��ġ");
		yield return new WaitForSeconds(1f);

		progress = 0.6f;
		Debug.Log("������ ��ġ");
		yield return new WaitForSeconds(1f);

		progress = 1.0f;
		Debug.Log("Scene Loading �Ϸ�");
		yield return new WaitForSeconds(0.5f);
	}
}