using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Debuff_State { None,NoColider,Surface,ZeroSpeed,Spring,Ice,Length }

public class Debuff : IDebuff
{
    public Debuff_State state;
    public Platform platform;
    public PhysicsMaterial2D[] debuff_PhysicsMaterials;

    public Debuff(int index = 0)
    {
        this.state = (Debuff_State)index;
        debuff_PhysicsMaterials = new PhysicsMaterial2D[(int)Debuff_State.Length];
        InitPhysicsList();
    }

    public void InitPhysicsList()
    {
        debuff_PhysicsMaterials[(int)Debuff_State.Ice] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Ice");
        debuff_PhysicsMaterials[(int)Debuff_State.Spring] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Spring");
    }

    public void DebugCurrentState()
    {
        Debug.Log(state.ToString());
    }

    public void SetState(int index)
    {
        state = (Debuff_State) index;
    }

    public void SetTrap(Platform platform)
    {
        if (this.platform != platform)
        {
            this.platform = platform;
        }
        BoxCollider2D boxCollider2D = platform.GetComponent<BoxCollider2D>();
        SurfaceEffector2D surfaceEffector2D = platform.GetComponent<SurfaceEffector2D>();

        Debug.Log($"�÷����� {state} ���� ��ġ");
        // �� state�� ����  AddComponent �Ǵ� Player ��ġ ��ȭ �Լ� ȣ�� 
        // �Ʒ� �ҽ��� �븮�ڸ� ���, platform ������ �����ǰ� ���� ����
        switch (state)
        {
            case Debuff_State.NoColider: boxCollider2D.isTrigger = true; break; // 1��(�浹ü ���ֱ�)�� ���
            case Debuff_State.Surface: surfaceEffector2D.enabled = true; break; // 2��(ǥ������Ʈ)�� ���
            case Debuff_State.Spring: boxCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Spring]; break;
            case Debuff_State.Ice: boxCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Ice]; break;
            case Debuff_State.None:
                {
                    boxCollider2D.sharedMaterial = null;
                    boxCollider2D.isTrigger = false;
                    surfaceEffector2D.enabled = false;
                    break;
                }
            default: Debug.Log($"{state}�� ���� ��������"); break;
            // 3��(�÷��̾� �ӵ� ���İ��� 0���� �����)�� ���
        }
    }

    public void DisableTrap()
    {
        state = Debuff_State.None;
        platform = null;
    }

    public IDebuff clone()
    {
        return this.MemberwiseClone() as IDebuff;
    }
}
