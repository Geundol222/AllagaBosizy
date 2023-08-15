using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetTrapUI : InGameUI, IPointerClickHandler
{
    Platform platform;
    Animator animator;
    Coroutine buttonCloseCoroutine;
    private UICloseArea closeArea;

    protected override void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
    }

    public void SetParentPlatform(Platform platform)
    {
        this.platform = platform;
        closeArea.Init(platform);
    }
     

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("������ġ");
        // ����� �ο� �Լ�ȣ��
        // ������ġ ȿ���� �߰� 23.08.14
        GameManager.Sound.PlaySound("Stage/JobsDone",Audio.SFX,gameObject.transform.position,2f);
        platform.OnClickSetTrap();
        platform.ClearBothPlatform();
        ExecuteSetTrapButtonClosing();
    }

    public void ExecuteSetTrapButtonClosing()
    {
        if (platform == null)
        {
            Debug.Log("SetTrapUI Platform is NULL");
            return;
        }
        buttonCloseCoroutine = StartCoroutine(HideSetTrapButton());
    }

    IEnumerator HideSetTrapButton()
    {
        closeArea.ClearPlatform(); // OnDisable �� �־� �Լ� ȣ���Ϸ�������.. �ڷ�ƾ�� WaitForSeconds 0.3f ������ ������ ��Ŵ.. �׷��� �̰��� ��ġ - 230801 02:19 AM 
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.3f);
        platform.HideSetTrapButton();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
