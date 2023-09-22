using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public enum SLOTSTAGE
    {
        EMPTY,
        FULL
    }

    public int id;              //���� ��ȣ ID
    public Item itemObject;     //������ Ŀ����
    public SLOTSTAGE state = SLOTSTAGE.EMPTY;   //Enum �� ����

    private void ChangeStateTo(SLOTSTAGE targetState)
    {//�ش� ������ ���°��� ��ȯ �����ִ� �Լ�
        state = targetState;
    }

    public void ItemGrabbed()
    {//RayCast�� ���ؼ� �������� �������
        Destroy(itemObject.gameObject);     //���� �������� ����
        ChangeStateTo(SLOTSTAGE.EMPTY);     //������ �� ����
    }
    public void CreatItem(int id)
    {
        string itemPath = "Prefabs/item_" + id.ToString("000");
        var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));

        itemGo.transform.SetParent(this.transform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        //������ �� ���� �Է�
        itemObject = itemGo.GetComponent<Item>();
        itemObject.Init(id, this);      //�Լ��� ���� �� �Է� this > Slot Class
        
        ChangeStateTo(SLOTSTAGE.FULL);  //���� �� ����
    }
}
