using UnityEngine;

public class MyObjectEventHandler : MonoBehaviour, IPrefabSpawnedListener
{
    private EventController eventController;

    private void Start()
    {
        // EventController 컴포넌트를 찾아서 가져옴
        eventController = FindObjectOfType<EventController>();

        // 이벤트 리스너로 등록
        eventController.RegisterListener(this);
    }

    public void OnPrefabSpawned(GameObject spawnedPrefab)
    {
        // Road_Prefabs_Event 프리팹이 생성되었을 때의 처리 로직
        if (spawnedPrefab.name == "Road_Prefabs_Event")
        {
            Debug.Log("Road_Prefabs_Event 프리팹이 생성되었습니다. 특정 물체를 감지하고 처리합니다.");

            // 도로 위 특정 물체를 감지하고 처리하는 로직을 추가
            Collider[] colliders = Physics.OverlapSphere(spawnedPrefab.transform.position, 1f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("MyObject"))
                {
                    Debug.Log("도로 위에 특정 물체를 감지했습니다: " + collider.gameObject.name);
                    // 여기서 해당 물체에 대한 처리 로직을 수행
                }
            }
        }
    }

    private void OnDestroy()
    {
        // 이벤트 리스너에서 해제
        if (eventController != null)
        {
            eventController.UnregisterListener(this);
        }
    }
}
