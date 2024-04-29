using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    private List<IPrefabSpawnedListener> listeners = new List<IPrefabSpawnedListener>();

    // �̺�Ʈ ������ ���
    public void RegisterListener(IPrefabSpawnedListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    // �̺�Ʈ ������ ����
    public void UnregisterListener(IPrefabSpawnedListener listener)
    {
        listeners.Remove(listener);
    }

    // ������ ���� �̺�Ʈ �˸�
    public void NotifyPrefabSpawned(GameObject spawnedPrefab)
    {
        foreach (var listener in listeners)
        {
            listener.OnPrefabSpawned(spawnedPrefab);
        }
    }
}