using UnityEngine;
using System.Collections;

public class Road_Remover : MonoBehaviour
{
    public float destroyDelay = 2f; // ���� ���� ���� �ð� (��)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // �±װ� "Car"�� ��ü�� Ʈ���ſ� �浹�ϸ� ���� ���� �ڷ�ƾ ����
            StartCoroutine(DestroyRoad());
        }
    }

    private IEnumerator DestroyRoad()
    {
        // destroyDelay �ð� ���� ��� �� ���� ����
        yield return new WaitForSeconds(destroyDelay);

        // �θ� ���� ������Ʈ(������) ����
        Destroy(gameObject.transform.parent.gameObject);
    }   
}