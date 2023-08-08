using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrollerDataManager : MonoBehaviour
{
    public TrollerPlayerController trollerPlayerController;         // ������ ��Ʈ�ѷ�
    public Platform currentPlatform;                                // ���� ���õ� �÷���
    public Platform prevPlatform;                                   // ������ ������ �÷���
    public bool canSetTrap;                                    // ���� ��ġ���ο� ���� Ŭ�� �������� Ȯ���ϴ� ����

    public Debuff Original_Debuff;                                  // clone ��ų Debuff Ŭ����
    public Queue<IDebuff> debuffQueue;                              // Debuff���� ���� Queue
    public PhysicsMaterial2D[] debuff_PhysicsMaterials;             // Debuff ȿ���� ���� �������׸����� ���� �迭

    public float debuffSetCoolTime = 5;                             // ���� ��ġ ��Ÿ��
    public int debuffQueueLength = 4;                               // Debuff�� ���� Queue�� �ִ�ũ��
    public int debuffCount { get { return debuffQueue.Count; } }    // ���� Queue�� ũ��

    public List<Platform> setTrapPlatforms;
    public int maxSetTrapPlatforms = 5;

    private void Awake()
    {
        Original_Debuff = new Debuff(0);
        debuffQueue = new Queue<IDebuff>();
        debuff_PhysicsMaterials = new PhysicsMaterial2D[(int)Debuff_State.Length];
        canSetTrap = true;
        setTrapPlatforms = new List<Platform>();
        InitPhysicsList();
    }

    public void InitPhysicsList()
    {
        debuff_PhysicsMaterials[(int)Debuff_State.Ice] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Ice");
        debuff_PhysicsMaterials[(int)Debuff_State.Spring] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Spring");
    }
}
