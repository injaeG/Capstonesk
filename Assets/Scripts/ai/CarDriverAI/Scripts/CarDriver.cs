using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour {

    public WheelCollider[] wheelColliders;
    public Transform[] wheelMeshes;
    public Transform centerOfMass; // 중심축을 위한 Transform 추가

    private Rigidbody rb; // Rigidbody 컴포넌트를 저장하기 위한 변수
    private float forwardAmount;
    private float turnAmount;

    [SerializeField] private float maxSteerAngle = 30f; // 최대 조향각
    [SerializeField] private float motorTorque = 1000f; // 모터 토크
    [SerializeField] private float forwardFrictionStiffness = 1.0f; // 전방 마찰 계수
    [SerializeField] private float sidewaysFrictionStiffness = 1.0f; // 측면 마찰 계수

    

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        if (centerOfMass != null) {
            rb.centerOfMass = centerOfMass.localPosition; // 중심축 설정
        }
    }

    private void Start() {
        AdjustWheelFriction();
    }

    private void Update() {
        for (int i = 0; i < wheelColliders.Length; i++) {
            if (i < 2) {
                wheelColliders[i].steerAngle = turnAmount * maxSteerAngle;
            }
            wheelColliders[i].motorTorque = forwardAmount * motorTorque;

            // Update wheel meshes positions
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);
            wheelMeshes[i].position = pos;
            wheelMeshes[i].rotation = quat;
        }
    }

    public void SetInputs(float forwardAmount, float turnAmount) {
        this.forwardAmount = forwardAmount;
        this.turnAmount = turnAmount;
    }

    public float GetSpeed() {
        return rb.velocity.magnitude; // 속도 계산 방법 수정
    }

    private void AdjustWheelFriction() {
        foreach (var wheelCollider in wheelColliders) {
            WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
            forwardFriction.stiffness = forwardFrictionStiffness;
            wheelCollider.forwardFriction = forwardFriction;

            WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
            sidewaysFriction.stiffness = sidewaysFrictionStiffness;
            wheelCollider.sidewaysFriction = sidewaysFriction;
        }
    }
}
