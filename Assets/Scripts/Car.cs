using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Car : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        // w ->앞
        if (Input.GetKey(KeyCode.W))
        {
            transform.position -= new Vector3(0.0f, 0.0f, 0.05f);
        }
        // s->뒤
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0.0f, 0.0f, 0.05f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(0.05f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position -= new Vector3(0.05f, 0.0f, 0.0f);
        }


    }
}