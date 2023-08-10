using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Platform platform;                                      // isClickable �Լ� ����� ���� Platform ����
    private List<Platform> setTrapPlatforms { get { return GameManager.TrollerData._setTrapPlatforms; } }
    private int setTrapPlatformsCount { get { return GameManager.TrollerData._setTrapPlatforms.Count; } }

    private bool canMouseAction
    {
        get
        {
            if (setTrapPlatformsCount < GameManager.TrollerData.maxSetTrapPlatforms && GameManager.TrollerData.canSetTrap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void Awake()
    {
        platform = GetComponent<Platform>();
    }

    private void Start()
    {
        StartCoroutine(TrollerControllerFindRoutine());
    }


    IEnumerator TrollerControllerFindRoutine()
    {
        yield return new WaitUntil(() => { return GameObject.Find("TrollerController(Clone)"); });
        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
            GameManager.TrollerData.trollerPlayerController = GameObject.Find("TrollerController(Clone)").GetComponent<TrollerPlayerController>();
        else
            yield break;
        yield break;
    }

    /// <summary>
    /// Ŭ�� �ȴٸ� clickAbleState �� true�� ..
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canMouseAction)
            return;
        if (platform.IsClickable)
        {
            platform.ShowSetTrapButton();
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
        if (!canMouseAction)
            return;
        platform.SwitchRenderColorEnter();
    }

}
