using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public float range = 3.0f;      //Ÿ�� ��Ÿ�
    public float fireRate = 1.0f;   //Ÿ�� �߻� ����
    public LayerMask IsEnemy;       //���̾� �ý������� ����

    public Collider[] colliderInRange;  //��Ÿ��� ������ collider �迭

    public List<EnemyController> enemiesInRange = new List<EnemyController>();  //��Ÿ� �ȿ� �̤Ӥ��� Enemy ������Ʈ ����Ʈ

    public float checkCounter;      //�ð� üũ��
    public float checkTime = 0.2f;  //0.2���� ����

    public bool enemiesUpdate;      //flag ������ üũ �Ϸ��ߴ��� ����

    // Start is called before the first frame update
    void Start()
    {
        checkCounter = checkTime;       //������ �ð��� CheckCounter �Է�
    }

    // Update is called once per frame
    void Update()
    {
        enemiesUpdate = false;

        checkCounter -= Time.deltaTime;     //0.2 > 0�ʰ� �� ������ �ð� ����

        if(checkCounter <= 0)               //0�� ���ϰ� �Ǿ��� ��
        {
            checkCounter = checkTime;       //0.2�ʷ� �ٽ� ����

            colliderInRange = Physics.OverlapSphere(transform.position, range, IsEnemy);    //�ڽ��� ��ġ, ������, ���̰��� ���ؼ� collider ����

            enemiesInRange.Clear();     //List �ʱ�ȭ (������ ������� ���� �ֱ� ������d

            foreach(Collider col in colliderInRange)
            {
                enemiesInRange.Add(col.GetComponent<EnemyController>());    //collider �迭�� �ִ� ������Ʈ�� list�� �ִ´�.
            }

            enemiesUpdate = true;
        }
    }
}
