using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    //���� ���� ���� ���� ������ �ִ� Ŭ����
    public int slotId;       //���� ��ȣ
    public int itemId;      //������ ��ȣ

    public void InitDummy(int slotId, int itemId)
    {//�μ��� ���� ���� Class �ʿ� �Է�
        this.slotId = slotId;
        this.itemId = itemId;
    }
}
