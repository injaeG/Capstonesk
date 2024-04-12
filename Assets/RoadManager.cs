using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadManager : MonoBehaviour
{
    public GameObject planePrefab; 
    public GameObject bigCar; 
    public GameObject transparentCube; 

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TransparentCube"))
        {
            SpawnNewRoad(); 
        }
    }

   
    void SpawnNewRoad()
    {
        
        Vector3 nextRoadPosition = CalculateNextRoadPosition();

        
        Instantiate(planePrefab, nextRoadPosition, Quaternion.identity);

        
        Instantiate(transparentCube, nextRoadPosition, Quaternion.identity);
    }

    
    Vector3 CalculateNextRoadPosition()
    {
        
        Vector3 hitPoint = transparentCube.transform.position;

        
        Vector3 direction = (hitPoint - bigCar.transform.position).normalized;
        float distance = 100f; 
        Vector3 nextRoadPosition = hitPoint + direction * distance;

        return nextRoadPosition;
    }
}