using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public Debuff Original_Debuff { get { return GameManager.TrollerData.Original_Debuff; } } // Clone�� ���� ����� ����

    int debuffQueueLength { get { return GameManager.TrollerData.debuffQueueLength; } set { GameManager.TrollerData.debuffQueueLength = value; } } // ����� ť ����
                                                                                                                                                   //int debuffCount { get { return GameManager.TrollerData.debuffCount; } }
    Queue<IDebuff> debuffQueue { get { return GameManager.TrollerData.debuffQueue; } set { GameManager.TrollerData.debuffQueue = value; } } // ����� ť

    public PhysicsMaterial2D[] debuff_PhysicsMaterials { get { return GameManager.TrollerData.debuff_PhysicsMaterials; } } // ����� �������׸��� �迭

    public TrapListUI trapListUI { get { return GameManager.TrollerData.trapListUI; } set { GameManager.TrollerData.trapListUI = value; } }

    public void UpdateTrapList()
    {
        if (GameManager.Team.GetTeam() != PlayerTeam.Troller)
            return;
        
        StartCoroutine(UpdateTrapListProcess());
    }

    IEnumerator UpdateTrapListProcess()
    {
        yield return new WaitUntil(() => { return GameManager.TrollerData.FindComplete; });

        Debuff[] debuffArray = new Debuff[debuffQueue.Count];
        debuffQueue.CopyTo(debuffArray, 0);
        trapListUI.UpdateList(debuffArray);

        yield break;
    }

    public void DebuffQueueInit()
    {
        if (GameManager.Team.GetTeam() != PlayerTeam.Troller)
            return;
        debuffQueue = new Queue<IDebuff>();

        debuffQueueLength = 4;

        for (int i = 0; i < debuffQueueLength; i++)
        {
            Debuff debuff = (Debuff)Original_Debuff.clone();
            debuff.SetState(Random.Range(1, (int)Debuff_State.Length));
            debuffQueue.Enqueue(debuff);
        }

        UpdateTrapList();
    }

    public void DebuffQueueEnqueue()
    {
        if (GameManager.Team.GetTeam() != PlayerTeam.Troller)
            return;
        if (debuffQueue.Count >= debuffQueueLength)
            return;

        Debuff debuff = (Debuff)Original_Debuff.clone();
        debuff.SetState(Random.Range(1, (int)Debuff_State.Length));
        debuffQueue.Enqueue(debuff);

        UpdateTrapList();
    }

    public Debuff CreateNoneStateDebuff()
    {
        Debuff debuff = (Debuff)Original_Debuff.clone();
        debuff.SetState((int)Debuff_State.None);

        return debuff;
    }

    public void SetTrap(Debuff debuff, Platform platform)
    {
        Collider2D platformCollider2D = platform.GetComponent<Collider2D>();
        SurfaceEffector2D surfaceEffector2D = platform.GetComponent<SurfaceEffector2D>();

        // �� state�� ����  AddComponent �Ǵ� Player ��ġ ��ȭ �Լ� ȣ�� 
        // �Ʒ� �ҽ��� �븮�ڸ� ���, platform ������ �����ǰ� ���� ����
        switch (debuff.state)
        {
            case Debuff_State.NoCollider: platformCollider2D.isTrigger = true; break; // 1��(�浹ü ���ֱ�)�� ���
            case Debuff_State.Surface: surfaceEffector2D.enabled = true; break; // 2��(ǥ������Ʈ)�� ���
            case Debuff_State.Spring: platformCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Spring]; break;
            case Debuff_State.Ice: platformCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Ice]; break;
            case Debuff_State.None:
                {
                    platformCollider2D.sharedMaterial = null;
                    platformCollider2D.isTrigger = false;
                    surfaceEffector2D.enabled = false;
                    break;
                }
            default: Debug.Log($"{debuff.state}�� ���� ��������"); break;
                // 3��(�÷��̾� �ӵ� ���İ��� 0���� �����)�� ���
        }
    }

    public void ShowCoolTimeUI()
    {
        trapListUI.ShowCoolTimeUI();
    }

    public void HideCoolTimeUI()
    {
        trapListUI.HideCoolTimeUI();
    }

    public void SetCoolTimeText(int coolTime)
    {
        trapListUI.SetCoolTimeText(coolTime);
    }

    public void DisableTrap(Debuff debuff)
    {
        debuff.state = Debuff_State.None;
    }
}
