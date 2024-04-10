using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public enum cameraBehaviours { NEW, OLD};
    public cameraBehaviours behaviours;

    public Rigidbody vehicleRigidbody;
    public Transform target;

    public float distance;
    public float height;

    [Range(0, 1)]
    public float smoothing;
    public float minVelocity;
    Vector3 orientation;
    public float sensX;
    public float sensY;


    float xRotation;
    float yRotation;


    public float wait = 1f;
    // Start is called before the first frame update
    void Start()
    {
        orientation = target.right;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }




    void FixedUpdate()
    {
        if (behaviours == cameraBehaviours.OLD)
        {

        }
        else
        {            
            if(vehicleRigidbody.velocity.magnitude > minVelocity && wait < 0)                
                orientation = vehicleRigidbody.velocity.normalized;
            else
                wait -= Time.deltaTime;

            Vector3 targetPosition = target.position + orientation * distance + Vector3.up * height;

            transform.position += (targetPosition - transform.position) * smoothing;

        }

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);


    }


}
