using UnityEngine;

public class CubeSpawnScript : MonoBehaviour
{
    public GameObject cubePrefab; // ������ Cube ������

    void Start()
    {
        // Cube �������� �����ϰ� Road_Prefabs_Event�� �ڽ����� ����
        GameObject cube = Instantiate(cubePrefab, transform.position + Vector3.up, Quaternion.identity);
        cube.transform.SetParent(transform); // Cube�� Road_Prefabs_Event�� �ڽ����� ����
    }
}