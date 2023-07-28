using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMouseHandler : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Color pointerOverColor;                // ���콺 ������ �÷����� �� �ٲ��� ��
    private Color pointerOutColor = Color.white;                    // ���콺 ������ ���������� �� �ٲ��� ��(����)
    private Renderer[] renderer;                                    // �ڽİ�ü �������� ���� �迭
    private Platform platform;                                      // isClickable �Լ� ����� ���� Platform ����

    private void Awake()
    {
        // �ڽ� ��ü�� Renderer ������Ʈ ���
        renderer = GetComponentsInChildren<Renderer>();
        platform = GetComponent<Platform>();
    }

    public void SetRenderer(bool state)
    {
        // ���� �ٲ� State�� false�� ������ �÷��� ������(
        if (state)
        {
            SwitchRenderColor(pointerOverColor);
        }
        else
        {
            SwitchRenderColor(pointerOutColor);
        }
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
        SwitchRenderColor(pointerOutColor);
        //Debug.Log($"Exit from {gameObject.name} !!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (platform.IsClickable)
        {
            SwitchRenderColor(pointerOverColor);
            //Debug.Log($"Enter the {gameObject.name} !!");
        }
    }

    public void SwitchRenderColor(Color color)
    {
        foreach (Renderer renderer in renderer)
        {
            if (renderer != null)
                renderer.material.color = color;
        }
    }
}
