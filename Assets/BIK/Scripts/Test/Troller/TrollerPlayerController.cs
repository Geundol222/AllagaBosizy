using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrollerPlayerController : MonoBehaviourPun
{
    [SerializeField] int debuffQueueLength;
    [SerializeField] int debuffCount;
    [SerializeField] public TMP_Text[] TrapListTexts;
    public Queue<IDebuff> debuffQueue;

    private Platform currentPlatform;
    public Platform _currentPlatform { get { return currentPlatform; } }
    private Platform prevPlatform;
    public Platform _prevPlatform { get { return prevPlatform; } }

    TrapListUI trapListUI;

    /// <summary>
    ///  230730 TODO  00:54 
    /// Ŭ���� ������ SetTrapUI �׸��� ������ Ŭ���� SetTrapUI�� �� 
    /// ���� �ٸ��ٸ� ������ Ŭ���� SetTrapUI�� Close ���ְ� Ŭ���� ������ SetTrapUI�� ���
    /// 
    /// �ٵ� ���� ���� ���� ������ DataManager���� ��������� �� �� ������ ..
    /// </summary>
    // Start is called before the first frame update

    private void Awake()
    {
        DebuffQueueInit();
        trapListUI = GameManager.UI.ShowInGameUI<TrapListUI>("UI/TrapList");
    }

    public void DebuffQueueInit()
    {
        debuffQueue = new Queue<IDebuff>();

        debuffQueueLength = 4;

        for (int i = 0; i < debuffQueueLength; i++)
        {
            Debuff debuff = new Debuff(Random.Range(1, (int)Debuff_State.Length));
            debuffQueue.Enqueue(debuff);
        }
    }

    public void DebuffQueueEnqueue()
    {
        if (debuffQueue.Count >= debuffQueueLength)
            return;
        Debuff debuff = new Debuff(Random.Range(1, (int)Debuff_State.Length - 1));
        debuffQueue.Enqueue(debuff);
    }
      
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
