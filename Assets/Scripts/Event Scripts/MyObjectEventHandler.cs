using UnityEngine;

public class MyObjectEventHandler : MonoBehaviour, IPrefabSpawnedListener
{
    private EventController eventController;

    private void Start()
    {
        // EventController ������Ʈ�� ã�Ƽ� ������
        eventController = FindObjectOfType<EventController>();

        // �̺�Ʈ �����ʷ� ���
        eventController.RegisterListener(this);
    }

    public void OnPrefabSpawned(GameObject spawnedPrefab)
    {
        // Road_Prefabs_Event �������� �����Ǿ��� ���� ó�� ����
        if (spawnedPrefab.name == "Road_Prefabs_Event")
        {
            Debug.Log("Road_Prefabs_Event �������� �����Ǿ����ϴ�. Ư�� ��ü�� �����ϰ� ó���մϴ�.");

            // ���� �� Ư�� ��ü�� �����ϰ� ó���ϴ� ������ �߰�
            Collider[] colliders = Physics.OverlapSphere(spawnedPrefab.transform.position, 1f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("MyObject"))
                {
                    Debug.Log("���� ���� Ư�� ��ü�� �����߽��ϴ�: " + collider.gameObject.name);
                    // ���⼭ �ش� ��ü�� ���� ó�� ������ ����
                }
            }
        }
    }

    private void OnDestroy()
    {
        // �̺�Ʈ �����ʿ��� ����
        if (eventController != null)
        {
            eventController.UnregisterListener(this);
        }
    }
}
