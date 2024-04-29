using UnityEngine;

public class CubeSpawnScript : MonoBehaviour
{
    public GameObject cubePrefab; // 생성할 Cube 프리팹

    void Start()
    {
        // Cube 프리팹을 생성하고 Road_Prefabs_Event의 자식으로 설정
        GameObject cube = Instantiate(cubePrefab, transform.position + Vector3.up, Quaternion.identity);
        cube.transform.SetParent(transform); // Cube를 Road_Prefabs_Event의 자식으로 설정
    }
}