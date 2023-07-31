using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollerPlayerController : MonoBehaviourPun
{
    private Platform currentPlatform;
    public Platform _currentPlatform { get { return currentPlatform; } }
    private Platform prevPlatform;
    public Platform _prevPlatform { get { return prevPlatform; } }

    /// <summary>
    ///  230730 TODO  00:54 
    /// Ŭ���� ������ SetTrapUI �׸��� ������ Ŭ���� SetTrapUI�� �� 
    /// ���� �ٸ��ٸ� ������ Ŭ���� SetTrapUI�� Close ���ְ� Ŭ���� ������ SetTrapUI�� ���
    /// 
    /// �ٵ� ���� ���� ���� ������ DataManager���� ��������� �� �� ������ ..
    /// </summary>
    // Start is called before the first frame update

    public void ClearBothPlatform()
    {
        currentPlatform = null;
        prevPlatform = null;
    }

    public void ClearCurrentPlatform()
    {
        currentPlatform = null;
    }

    public void SetCurrentPlatform(Platform platform)
    {
        currentPlatform = platform;
    }

    public void SetPrevPlatform(Platform platform)
    {
        prevPlatform = platform;
    }
}
