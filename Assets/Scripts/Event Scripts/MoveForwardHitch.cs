using UnityEngine;

public class MoveForwardHitch : MonoBehaviour
{
    public float fixedY = 1.0f; // ������ Y ��ǥ ��

    void Update()
    {
        // 'Car' �±װ� �ִ� ������Ʈ�� ã���ϴ�.
        GameObject car = GameObject.FindGameObjectWithTag("Car");

        // 'Car' �±װ� �ִ� ������Ʈ�� �ִ� ��쿡�� �̵��մϴ�.
        if (car != null)
        {
            // ������Ʈ�� 'Car' ������Ʈ�� �������� ȸ����ŵ�ϴ�.
            Vector3 lookDirection = new Vector3(car.transform.position.x, transform.position.y, car.transform.position.z);
            transform.LookAt(lookDirection);
        }
    }
}