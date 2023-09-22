using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Slot[] slots;                //���� ��Ʈ�ѷ������� ���� �迭 ����

    private Vector3 _target;
    private ItemInfo carryingItem;      //��� �ִ� ������ ���� �� ����

    private Dictionary<int, Slot> slotDictionary;   //���� ���̵�, ���� Ŭ���� �����ϱ� ���� �ڷᱸ��


    private void Start()
    {
        slotDictionary = new Dictionary<int, Slot>();   //�ʱ�ȭ

        for (int i = 0; i < slots.Length; i++)
        {//�� ������ ID�� �����ϰ� ��ųʸ��� �߰�
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //���콺 ������
        {
            SendRayCast();
        }
        if (Input.GetMouseButton(0) && carryingItem)    //��� �̵���ų ��
        {
            OnItemSelected();
        }
        if (Input.GetMouseButtonUp(0))  //���콺 ��ư�� ������
        {
            SendRayCast();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceRandomItem();
        }
    }

    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<Slot>();  //����ĳ��Ʈ�� ���� ���� ���� ĭ
            if(slot.state == Slot.SLOTSTAGE.FULL && carryingItem == null)
            {      //������ ���Կ��� �������� ��´�.
                string itemPath = "Prefabs/item_Graddbed_" + slot.itemObject.id.ToString("000");
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath)); //������ ����
                itemGo.transform.SetParent(this.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one * 2;

                carryingItem = itemGo.GetComponent<ItemInfo>(); //���� ���� �Է�
                carryingItem.InitDummy(slot.id, slot.itemObject.id);

                slot.ItemGrabbed();

            }
            else if (slot.state == Slot.SLOTSTAGE.EMPTY && carryingItem != null)
            {//�� ���Կ� ������ ��ġ
                slot.CreatItem(carryingItem.itemId);    //��� �ִ� �� ���� ��ġ�� ����
                Destroy(carryingItem.gameObject);      //��� �ִ� �� �ı�
            }
            else if (slot.state == Slot.SLOTSTAGE.FULL && carryingItem != null)
            {//Checking �� ����
                if(slot.itemObject.id == carryingItem.itemId)
                {
                    OnItemMergerWithTarget(slot.id);    //���� �Լ� ȣ��
                }
                else
                {

                    OnItemCarryFail();      //������ ��ġ ����
                }
            }
        }
        else
        {
            if (!carryingItem) return;

            OnItemCarryFail();  //������ ��ġ ����
        }
    }

    void OnItemSelected()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //��ǥ��ȯ
        _target.z = 0;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }

    void OnItemMergerWithTarget(int targetSlotId)
    {
        var slot = GetSiotById(targetSlotId);
        Destroy(slot.itemObject.gameObject);        //���Կ� �ִ� ��ü �ı�
        slot.CreatItem(carryingItem.itemId + 1);    //���Կ� ���� ��ȣ ��ü ����
        Destroy(carryingItem.gameObject);           //��� �ִ� ��ü �ı�
    }

    void OnItemCarryFail()
    {//������ ��ġ ���� �� ����
        var slot = GetSiotById(carryingItem.slotId);    //���� ��ġ Ȯ��
        slot.CreatItem(carryingItem.itemId);            //�ش� ���Կ� �ٽ� ����
        Destroy(carryingItem.gameObject);               //��� �ִ� ��ü �ı�
    }

    void PlaceRandomItem()
    {//������ ���Կ� ������ ��ġ
        if(AllSlotsOccupied())
        {
            return;
        }
        var rand = UnityEngine.Random.Range(0, slots.Length);   //����Ƽ �����Լ��� �����ͼ� 0 ~ �迭 ũ�� ���� ��
        var slot = GetSiotById(rand);
        while(slot.state == Slot.SLOTSTAGE.FULL)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSiotById(rand);
        }
        slot.CreatItem(0);
    }

    bool AllSlotsOccupied()
    {//��� ������ ä���� �ִ��� Ȯ��
        foreach(var slot in slots)      //foreach ���� ���ؼ� ���� �迭 �˻�
        {
            if(slot.state == Slot.SLOTSTAGE.EMPTY)      //����ִ��� Ȯ��
            {
                return false;
            }
        }
        return true;
    }

    Slot GetSiotById(int id)
    {//���� ���̵�� ��ųʸ����� ���� Ŭ���� ����
        return slotDictionary[id];
    }
}
