using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollerPlayerController : MonoBehaviourPun
{
    [SerializeField] int debuffQueueLength;
    public Queue<IDebuff> debuffQueue;

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

    private void Awake()
    {
        DebuffQueueInit();
    }

    public void DebuffQueueInit()
    {
        debuffQueue = new Queue<IDebuff>();

        debuffQueueLength = 4;

        for (int i = 0; i < debuffQueueLength; i++)
        {
            Debuff debuff = new Debuff(Random.Range(1, (int)Debuff_State.Length));
            debuffQueue.Enqueue(debuff);
            Debug.Log($"{debuff.state.ToString()}�� {i + 1} �� ° �־���");
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
