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

    public int id;              //슬롯 번호 ID
    public Item itemObject;     //선언한 커스텀
    public SLOTSTAGE state = SLOTSTAGE.EMPTY;   //Enum 값 선언

    private void ChangeStateTo(SLOTSTAGE targetState)
    {//해당 슬롯의 상태값을 변환 시켜주는 함수
        state = targetState;
    }

    public void ItemGrabbed()
    {//RayCast를 통해서 아이템을 잡았을떄
        Destroy(itemObject.gameObject);     //기존 아이템을 삭제
        ChangeStateTo(SLOTSTAGE.EMPTY);     //슬롯은 빈 상태
    }
    public void CreatItem(int id)
    {
        string itemPath = "Prefabs/item_" + id.ToString("000");
        var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));

        itemGo.transform.SetParent(this.transform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        //아이템 값 정보 입력
        itemObject = itemGo.GetComponent<Item>();
        itemObject.Init(id, this);      //함수를 통한 값 입력 this > Slot Class
        
        ChangeStateTo(SLOTSTAGE.FULL);  //슬롯 찬 상태
    }
}
