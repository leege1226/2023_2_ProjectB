using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Slot[] slots;                //게임 컨트롤러에서는 슬롯 배열 관리

    private Vector3 _target;
    private ItemInfo carryingItem;      //잡고 있는 아이템 정보 값 관리

    private Dictionary<int, Slot> slotDictionary;   //슬롯 아이디, 슬롯 클래스 관리하기 위한 자료구조


    private void Start()
    {
        slotDictionary = new Dictionary<int, Slot>();   //초기화

        for (int i = 0; i < slots.Length; i++)
        {//각 슬롯의 ID를 설정하고 딕셔너리에 추가
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //마우스 누를때
        {
            SendRayCast();
        }
        if (Input.GetMouseButton(0) && carryingItem)    //잡고 이동시킬 때
        {
            OnItemSelected();
        }
        if (Input.GetMouseButtonUp(0))  //마우스 버튼을 놓을때
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
            var slot = hit.transform.GetComponent<Slot>();  //레이캐스트를 통해 나온 슬롯 칸
            if(slot.state == Slot.SLOTSTAGE.FULL && carryingItem == null)
            {      //선택한 슬롯에서 아이템을 잡는다.
                string itemPath = "Prefabs/item_Graddbed_" + slot.itemObject.id.ToString("000");
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath)); //아이템 생성
                itemGo.transform.SetParent(this.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one * 2;

                carryingItem = itemGo.GetComponent<ItemInfo>(); //슬롯 정보 입력
                carryingItem.InitDummy(slot.id, slot.itemObject.id);

                slot.ItemGrabbed();

            }
            else if (slot.state == Slot.SLOTSTAGE.EMPTY && carryingItem != null)
            {//빈 슬롯에 아이템 배치
                slot.CreatItem(carryingItem.itemId);    //들고 있는 것 슬롯 위치에 생성
                Destroy(carryingItem.gameObject);      //잡고 있던 것 파괴
            }
            else if (slot.state == Slot.SLOTSTAGE.FULL && carryingItem != null)
            {//Checking 후 병합
                if(slot.itemObject.id == carryingItem.itemId)
                {
                    OnItemMergerWithTarget(slot.id);    //병합 함수 호출
                }
                else
                {

                    OnItemCarryFail();      //아이템 배치 실패
                }
            }
        }
        else
        {
            if (!carryingItem) return;

            OnItemCarryFail();  //아이템 배치 실패
        }
    }

    void OnItemSelected()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //좌표변환
        _target.z = 0;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }

    void OnItemMergerWithTarget(int targetSlotId)
    {
        var slot = GetSiotById(targetSlotId);
        Destroy(slot.itemObject.gameObject);        //슬롯에 있는 물체 파괴
        slot.CreatItem(carryingItem.itemId + 1);    //슬롯에 다음 번호 물체 생성
        Destroy(carryingItem.gameObject);           //잡고 있는 물체 파괴
    }

    void OnItemCarryFail()
    {//아이템 배치 실패 시 실행
        var slot = GetSiotById(carryingItem.slotId);    //슬롯 위치 확인
        slot.CreatItem(carryingItem.itemId);            //해당 슬롯에 다시 생성
        Destroy(carryingItem.gameObject);               //잡고 있는 물체 파괴
    }

    void PlaceRandomItem()
    {//랜덤한 슬롯에 아이템 배치
        if(AllSlotsOccupied())
        {
            return;
        }
        var rand = UnityEngine.Random.Range(0, slots.Length);   //유니티 랜덤함수를 가져와서 0 ~ 배열 크기 사이 값
        var slot = GetSiotById(rand);
        while(slot.state == Slot.SLOTSTAGE.FULL)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSiotById(rand);
        }
        slot.CreatItem(0);
    }

    bool AllSlotsOccupied()
    {//모든 슬롯이 채워져 있는지 확인
        foreach(var slot in slots)      //foreach 문을 통해서 슬롯 배열 검사
        {
            if(slot.state == Slot.SLOTSTAGE.EMPTY)      //비어있는지 확인
            {
                return false;
            }
        }
        return true;
    }

    Slot GetSiotById(int id)
    {//슬롯 아이디로 딕셔너리에서 슬롯 클래스 리턴
        return slotDictionary[id];
    }
}
