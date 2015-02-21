using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	private CameraController mainCamera;
	public GameObject domino;

	void Start()
	{
		GameObject mainCameraObject = GameObject.FindWithTag ("MainCamera");
		if (mainCameraObject != null) {
			mainCamera = mainCameraObject.GetComponent <CameraController> ();
		} else {
			Debug.Log ("Captain, we've lost the camera");
			return;
		}
	}

	void Update()
	{
		if (mainCamera.isOrthographic) {
			if (Input.GetButtonDown ("Fire1")) {

				//RaycastHit hit;
				//Ray ray = Camera.ScreenPointToRay (Input.mousePosition);
				//raycast part
				//Vector3 = Input.mousePosition;

				//Instantiate (domino, transform.position, transform.rotation);

			}
		}
	}
}