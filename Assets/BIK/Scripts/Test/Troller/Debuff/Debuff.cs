using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Debuff_State { None,NoColider,Surface,ZeroSpeed,Length }

public class Debuff : MonoBehaviour,IDebuff
{
    public Debuff_State state;
    public Platform platform;

    public Debuff(int index)
    {
        this.state = (Debuff_State)index;
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
        this.platform = platform;
        Debug.Log($"�÷����� {state} ���� ��ġ");
        // �� state�� ����  AddComponent �Ǵ� Player ��ġ ��ȭ �Լ� ȣ�� 
        // �Ʒ� �ҽ��� �븮�ڸ� ���, platform ������ �����ǰ� ���� ����
        switch (state)
        {
            case Debuff_State.NoColider: platform.gameObject.GetComponent<PlatformEffector2D>().enabled = false; break; // 1��(�浹ü ���ֱ�)�� ���
            case Debuff_State.Surface: //platform.gameObject.GetComponent<SurfaceEffector2D>().enabled = true; break; // 2��(ǥ������Ʈ)�� ���
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
