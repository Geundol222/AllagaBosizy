using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMouseHandler : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{

    private Platform platform;                                      // isClickable �Լ� ����� ���� Platform ����
                                
    private void Awake()
    {
        // �ڽ� ��ü�� Renderer ������Ʈ ���
        platform = GetComponent<Platform>();
    }
      
    /// <summary>
    /// Ŭ�� �ȴٸ� clickAbleState �� true�� ..
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (platform.IsClickable)
        {
            Debug.Log($"Clicked {gameObject.name} !!");
        }                
    }
      
    /// <summary>
    /// ���콺�� �ٱ����� �����ٸ� 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        platform.SwitchRenderColorExit();
    }
      
    public void OnPointerEnter(PointerEventData eventData)
    {
        platform.SwitchRenderColorEnter();
    }
      
}
