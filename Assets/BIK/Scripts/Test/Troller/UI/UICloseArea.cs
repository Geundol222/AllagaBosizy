using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICloseArea : MonoBehaviourPun, IPointerClickHandler
{
    Platform platform;

    public void Init(Platform platform)
    {
        Debug.Log("1_ ù����_ platform �ʱ�ȭ");
        this.platform = platform;
    }

    public void ClearPlatform()
    {
        platform = null;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (platform == null)
        {
            return;
        }
        platform.ClearBothPlatform();
        platform._setTrapUI.ExecuteSetTrapButtonClosing();
    }

}
