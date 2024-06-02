using UnityEngine;
using UnityEngine.Splines;

public class SplineRoadGenerator : MonoBehaviour
{
    public GameObject roadPrefab; // 도로 프리팹
    public SplineContainer splineContainer; // 스플라인 컨테이너
    public int segmentCount = 10; // 세그먼트 개수

    void Start()
    {
        GenerateRoad();
    }

    void GenerateRoad()
    {
        if (splineContainer == null || roadPrefab == null)
        {
            Debug.LogError("스플라인 컨테이너와 도로 프리팹이 설정되지 않았습니다.");
            return;
        }

        Spline spline = splineContainer.Spline;
        float splineLength = spline.GetLength();
        float segmentLength = splineLength / segmentCount;

        for (int i = 0; i < segmentCount; i++)
        {
            float t1 = (i * segmentLength) / splineLength;
            float t2 = ((i + 1) * segmentLength) / splineLength;

            Vector3 startPos = spline.EvaluatePosition(t1);
            Vector3 endPos = spline.EvaluatePosition(t2);
            CreateRoadSegment(startPos, endPos);
        }
    }

    void CreateRoadSegment(Vector3 start, Vector3 end)
    {
        Vector3 middle = (start + end) / 2;
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        GameObject roadSegment = Instantiate(roadPrefab, middle, Quaternion.LookRotation(direction));
        roadSegment.transform.localScale = new Vector3(roadSegment.transform.localScale.x, roadSegment.transform.localScale.y, distance);
    }
}
