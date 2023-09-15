using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    private void Awake()                    //Awake 시점에서 인스턴스 검사
    {
        if (_instance == null)                //인스턴스가 없을 경우
        {
            _instance = this as T;         //지금 인스턴스를 Static에 입력
            DontDestroyOnLoad(gameObject);  //DontDestroyOnLoad 파괴되지 않는 오브젝트로 설정
        }
        else if (_instance != this)
        {
            Destroy(gameObject);            //기존에 인스턴스가 있는 경우 파괴 시킨다.
        }
    }
}
