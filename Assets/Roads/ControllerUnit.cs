using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUnit : MonoBehaviour {

	public GameObject[] roadPrefabs;
	public GameObject AICar;
	private GameObject[] allRoads = new GameObject[5];
	private int roadCounter;
	private int[] rotateRoad = {0,0};

	private Transform playerTransform;
	private bool deletefirstRoad = true;
	private float roadLenght = 10.0F;
	private int screenOnRoad = 2;

	private bool XorZ = false; // x: false z: true
	private bool goForward = false; // false:negatif eksen true: pozitif eksen
	private bool isTurn = false;
	private bool isRotate = false;

	private Vector3 lastTrack;


	
	void Start () {
		
		playerTransform = GameObject.FindGameObjectWithTag ("Car").transform;
		lastTrack = GameObject.FindGameObjectWithTag ("Road").transform.position;

		roadCounter = 0;
		XorZ = false;
		goForward = true;
		isRotate = false;
		
		for (int i = 0; i < allRoads.Length; i++) {
            		allRoads[i] = spawnRoad(controlCrossRoad());
		}
 

	}

	int controlCrossRoad(){
		int randomRoadType = Random.Range (0, 3);
		if (randomRoadType == 0) {
			return 0;
		}
		if (randomRoadType == 1 && rotateRoad [0] == 1 && rotateRoad [1] == 1) {
			rotateRoad [1] = 2;
			return 2;
		} else if (randomRoadType == 2 && rotateRoad [0] == 2 && rotateRoad [1] == 2) {
			rotateRoad [1] = 1;
			return 1;
		} else {
			rotateRoad [0] = rotateRoad [1];
			rotateRoad [1] = randomRoadType;
			return randomRoadType;
		}

	}

	void Update () {

        //Mathf.Abs(playerTransform.position.z - lastTrack.z) < screenOnRoad * roadLenght || Mathf.Abs(playerTransform.position.x - lastTrack.x) < screenOnRoad * roadLenght
		if (Vector3.Distance(playerTransform.position,lastTrack) < screenOnRoad * roadLenght) {
            
           		Destroy(allRoads[roadCounter]);
			allRoads[roadCounter]= spawnRoad(controlCrossRoad());
			roadCounter = (roadCounter + 1) % allRoads.Length;
			if (deletefirstRoad) {
				Destroy (GameObject.FindGameObjectWithTag ("Road"));
				deletefirstRoad = false;
			}
		}	

	}

	GameObject spawnRoad(int roadType){
		
		GameObject road;
		road = Instantiate (roadPrefabs [roadType]) as GameObject;
		road.transform.SetParent (transform);
		Vector3 temp = lastTrack;
		int vectorSize = (int)roadLenght;

		if (!goForward) {
			vectorSize = -vectorSize;
		}

		if (isRotate && roadType == 0) {
			road.transform.Rotate (new Vector3 (0, 90, 0));	
		}

		if (XorZ) {
			temp +=  new Vector3 (0, 0, vectorSize);
		} else {
			temp +=  new Vector3 (vectorSize, 0, 0);
		}
		if (roadType == 0) {
			GameObject aicar;
			aicar = Instantiate (AICar) as GameObject;
			if(XorZ == false && goForward == true){
				aicar.transform.Rotate (new Vector3 (0, 0, 0));
			} else if(XorZ == true && goForward == true){
				aicar.transform.Rotate (new Vector3 (0, -90, 0));
			} else if(XorZ == false && goForward == false){
				aicar.transform.Rotate (new Vector3 (0, 180, 0));
			} else if(XorZ == true && goForward == false){
				aicar.transform.Rotate (new Vector3 (0, 90, 0));
			}
			aicar.transform.position = temp;
		}
		road.transform.position = temp;

		if (roadType == 1) { // sola dönüş
			if(XorZ == false && goForward == true){
				XorZ = true; goForward = true;
				temp += new Vector3 (17.86F, 0.08726203F, 31.479F);
				road.transform.Rotate (new Vector3 (0, 0, 0));
			} else if(XorZ == true && goForward == true){
				XorZ = false; goForward = false;
				temp += new Vector3 (-31.479F, 0.08726203F, 17.86F);
				road.transform.Rotate (new Vector3 (0, -90, 0));
			} else if(XorZ == false && goForward == false){
				XorZ = true; goForward = false;
				temp += new Vector3 (-17.86F, 0.08726203F, -31.479F);
				road.transform.Rotate (new Vector3 (0, 180, 0));
			} else if(XorZ == true && goForward == false){
				XorZ = false; goForward = true;
				temp += new Vector3 (31.479F, 0.08726203F, -17.86F);
				road.transform.Rotate (new Vector3 (0, 90, 0));
			}
			isRotate = !isRotate;
			// sola dönüş
		} else if(roadType == 2){ // sağa dönüş


			if(XorZ == false && goForward == true){
				XorZ = true; goForward = false;
				temp += new Vector3 (17.64F, 0.08726096F, -32.459F);
				road.transform.Rotate (new Vector3 (0, 0, 0));
			} else if(XorZ == true && goForward == false){
				XorZ = false; goForward = false;
				temp += new Vector3 (-32.459F, 0.08726096F, -17.64F);
				road.transform.Rotate (new Vector3 (0, 90, 0));
			} else if(XorZ == false && goForward == false){
				XorZ = true; goForward = true;
				temp += new Vector3 (-17.64F, 0.08726096F, 32.459F);
				road.transform.Rotate (new Vector3 (0, 180, 0));
			} else if(XorZ == true && goForward == true){
				XorZ = false; goForward = true;
				temp += new Vector3 (32.459F, 0.08726096F, 17.64F);
				road.transform.Rotate (new Vector3 (0, -90, 0));
			}
			isRotate = !isRotate;
			// sağa dönüş
		}
		lastTrack = temp;
		return road;
	}
}
