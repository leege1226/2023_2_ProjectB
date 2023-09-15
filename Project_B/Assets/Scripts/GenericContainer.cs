using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericContainer<T>    //제네릭 형식 Class로 사용하기 위해서 선언
{
    private T[] items;              //Item 배열 선언
    private int currentlndex = 0;   //Item 인덱스

    public GenericContainer(int capacity)   //class에서 class이름과 같은 함수는 보통 생성자
    {
        items = new T[capacity];
    }

    public void Add(T item)                 //이 컨테이너에 값을 넣는다.
    {
        if(currentlndex < items.Length)     //컨테이너 배열 칸 이상 될경우 막는다.
        {
            items[currentlndex] = item;     //받은 값을 배열에 넣는다.
            currentlndex++;                 //인덱스를 증가시킨다.
        }
    }

    public T[] GetItems()                   //배열에 있는 값을 리턴
    {
        return items;
    }

}
