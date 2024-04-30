using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float fixedYRotation = 90.0f; // 고정시킬 Y축 회전값

    void Start()
    {
        // 오브젝트의 회전값을 설정합니다.
        // Quaternion.Euler를 사용하여 X, Y, Z 각도에서 회전을 나타내는 쿼터니언을 생성합니다.
        // 여기에서는 Y축 회전만 적용합니다.
        transform.rotation = Quaternion.Euler(0, fixedYRotation, 0);
    }
}
