using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericContainer<T>    //���׸� ���� Class�� ����ϱ� ���ؼ� ����
{
    private T[] items;              //Item �迭 ����
    private int currentlndex = 0;   //Item �ε���

    public GenericContainer(int capacity)   //class���� class�̸��� ���� �Լ��� ���� ������
    {
        items = new T[capacity];
    }

    public void Add(T item)                 //�� �����̳ʿ� ���� �ִ´�.
    {
        if(currentlndex < items.Length)     //�����̳� �迭 ĭ �̻� �ɰ�� ���´�.
        {
            items[currentlndex] = item;     //���� ���� �迭�� �ִ´�.
            currentlndex++;                 //�ε����� ������Ų��.
        }
    }

    public T[] GetItems()                   //�迭�� �ִ� ���� ����
    {
        return items;
    }

}
