using UnityEngine;

public class CollisionActivator : MonoBehaviour
{
    public GameObject[] objectsToActivate; // 활성화할 오브젝트 배열
    public int collisionLayer = 0; // 충돌할 레이어 인덱스

    private void OnCollisionEnter(Collision collision)
    {
        if (IsOnLayer(collision.gameObject, collisionLayer))
        {
            ActivateObjects();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOnLayer(other.gameObject, collisionLayer))
        {
            ActivateObjects();
        }
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }

    private bool IsOnLayer(GameObject go, int layer)
    {
        return ((1 << go.layer) & (1 << layer)) != 0;
    }
}
