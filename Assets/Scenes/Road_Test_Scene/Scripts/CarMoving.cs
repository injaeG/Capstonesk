using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoving : MonoBehaviour
{
    float timer;
    float waitingTime;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        waitingTime = 0.05f;
        
    }

    public float speed = 0.001f;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, -speed));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(speed, 0, 0));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, speed));
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(-speed, 0, 0));
            }

            timer = 0;
        }

        
    }
}