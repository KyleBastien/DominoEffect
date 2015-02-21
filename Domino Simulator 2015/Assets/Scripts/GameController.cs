using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private CameraController mainCamera;
	public GameObject domino;
	int floorMask;
	
	void Start()
	{
		floorMask = LayerMask.GetMask ("Floor");
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

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitPoint;
				
				if(Physics.Raycast(ray, out hitPoint, 200f, floorMask ))
				{
					Vector3 newHit = new Vector3(hitPoint.point.x, hitPoint.point.y + .5f, hitPoint.point.z);
					Quaternion rotate = Quaternion.identity;
					if(!Physics.CheckSphere(newHit,.27f))
					{
						Instantiate(domino, newHit, rotate);
					}

				}
				
			}
		}
	}
}
