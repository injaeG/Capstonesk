using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    private List<IPrefabSpawnedListener> listeners = new List<IPrefabSpawnedListener>();

    // 이벤트 리스너 등록
    public void RegisterListener(IPrefabSpawnedListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    // 이벤트 리스너 해제
    public void UnregisterListener(IPrefabSpawnedListener listener)
    {
        listeners.Remove(listener);
    }

    // 프리팹 생성 이벤트 알림
    public void NotifyPrefabSpawned(GameObject spawnedPrefab)
    {
        foreach (var listener in listeners)
        {
            listener.OnPrefabSpawned(spawnedPrefab);
        }
    }
}