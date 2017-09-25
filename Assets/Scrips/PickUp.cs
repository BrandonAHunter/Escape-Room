using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PickUp : MonoBehaviour {
	public GameObject mainCamera;
	bool carrying;
	GameObject carriedObject;
	public float distance = 2;
	public float smooth = 4;
	public float reach = 2;
	bool doorOpen = false;
	public GUIStyle InteractTextStyle;

	private string InteractText = "";
	private Rect interactTextRect;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (carrying) {
			carry (carriedObject);
			checkDrop ();
		} else {
			interact ();
		}
	}

	void carry(GameObject o){
		o.transform.position = Vector3.Lerp (o.transform.position, 
			mainCamera.transform.position + mainCamera.transform.forward * distance, 
			Time.deltaTime * smooth);
		o.transform.rotation = Quaternion.identity;
	}


	void interact(){
		
		int x = Screen.width / 2;
		int y = Screen.height / 2;
		Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay (new Vector3(x,y));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, reach)) {
			if (hit.collider.tag == "Pickup") {
				InteractText = "Press E to Pick Up";
				Vector2 textSize = InteractTextStyle.CalcSize (new GUIContent (InteractText));
				interactTextRect = new Rect (Screen.width / 2 - textSize.x / 2, Screen.height - (textSize.y + 5), textSize.x, textSize.y);
				if (Input.GetKeyDown (KeyCode.E)) {
					Debug.Log ("Picking Up");
					Pickupable p = hit.collider.GetComponent<Pickupable> ();
					if (p != null) {
						carrying = true;
						carriedObject = p.gameObject;
						p.gameObject.GetComponent<Rigidbody> ().useGravity = false;
					}
				}
			} else if (hit.collider.tag == "Stairs") {
				InteractText = "Press E to go down stairs";
				Vector2 textSize = InteractTextStyle.CalcSize (new GUIContent (InteractText));
				interactTextRect = new Rect (Screen.width / 2 - textSize.x / 2, Screen.height - (textSize.y + 5), textSize.x, textSize.y);
				if (Input.GetKeyDown (KeyCode.E)) {
					Debug.Log ("Changing scenes");
					SceneManager.LoadScene ("Levels/Level_02");
				}
			} else if (hit.collider.tag == "Door") {
				InteractText = "Press E to Open Door";
				Vector2 textSize = InteractTextStyle.CalcSize (new GUIContent (InteractText));
				interactTextRect = new Rect (Screen.width / 2 - textSize.x / 2, Screen.height - (textSize.y + 5), textSize.x, textSize.y);
				if (hit.collider.gameObject.GetComponent<SimpleDoorTrigger> ()) {
					hit.collider.gameObject.GetComponent<SimpleDoorTrigger> ().HandleUserInput ();
				}	
			} else {
				InteractText = "";
				Debug.Log ("nothing happening, You Hit " + hit.collider.gameObject.name);
			}
		} else {
			InteractText = "";
		}
	}

	void checkDrop(){
		if (Input.GetKeyDown (KeyCode.E)) {
			Debug.Log ("Pressed E to drop up");
			dropObject ();
		}
	}

	void dropObject(){
		carrying = false;
		carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		carriedObject = null;
	}

	void OnGUI(){
		GUI.Label(interactTextRect, InteractText, InteractTextStyle);
	}

}
