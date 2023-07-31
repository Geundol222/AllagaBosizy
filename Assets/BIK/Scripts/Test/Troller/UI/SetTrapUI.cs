using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetTrapUI : InGameUI, IPointerClickHandler
{
    Platform platform;
    Animator animator;
    Coroutine buttonCloseCoroutine;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void SetParentPlatform(Platform platform)
    {
        this.platform = platform;
    }
     

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("������ġ");
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
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.3f);
        platform.HideSetTrapButton();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
