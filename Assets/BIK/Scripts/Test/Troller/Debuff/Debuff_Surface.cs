using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff_Surface : MonoBehaviour, IDebuff
{
    // ���� ������� ����
    public IDebuff clone()
    {
        return this.MemberwiseClone() as IDebuff;
    }
}
