using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Debuff_HeavyGravity : MonoBehaviour, IDebuff
{
    // ���� ������� ����
    public IDebuff clone()
    {
        return this.MemberwiseClone() as IDebuff;
    }
}
