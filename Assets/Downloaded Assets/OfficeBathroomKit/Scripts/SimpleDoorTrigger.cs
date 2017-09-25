using UnityEngine;
using System.Collections;

public class SimpleDoorTrigger : MonoBehaviour {
	public Transform Door;
	public float OpenAngleAmount = 88.0f;
	public float SmoothRotation;

	private bool init = false;
	private bool hasEntered = false;
	private bool doorOpen = false;
	private Vector3 startAngle;
	private Vector3 openAngle;
		
	void Start () {
		//Check if Door Game Object is properly assigned
		if(Door == null){
			Debug.LogError (this + " :: Door Object Not Defined!");
		}
		
		//Init Start and Open door angles
		startAngle = Door.eulerAngles;
		openAngle = new Vector3(startAngle.x, startAngle.y + OpenAngleAmount, startAngle.z);
		
		init = true;
	}
		
	void Update () {
		if(!init)
			return;
		
		HandleDoorRotation();
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			hasEntered = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		hasEntered = false;
	}
	
	public void HandleDoorRotation(){
		if(!doorOpen)
			Door.rotation = Quaternion.Euler(Vector3.Lerp(Door.eulerAngles, startAngle, Time.deltaTime * SmoothRotation));
		else
			Door.rotation = Quaternion.Euler(Vector3.Lerp(Door.eulerAngles, openAngle, Time.deltaTime * SmoothRotation));
	}

	public void HandleUserInput(){
		if(Input.GetKeyDown(KeyCode.E)/* && hasEntered*/){
			doorOpen = !doorOpen;
		}			
	}
}