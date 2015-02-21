using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{

	public float rotateSpeed;
	private float shiftSpeed;
	private Vector3 oldPosition;
	private Quaternion oldRotation;
	private Vector3 tempPosition;
	private Quaternion tempRotation;
	

	void Start()
	{
		shiftSpeed = 1.0f;
		rotateSpeed = 3.0f;
		oldPosition = new Vector3 (0.0f, 20.0f, -20.0f);
		oldRotation = new Quaternion (0.4f, 0.0f, 0.0f, 0.9f);
	}

	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		float moveUp = 0.0f;
		float rotateHorizontal = Input.GetAxis ("Mouse X");
		float rotateVertical = Input.GetAxis ("Mouse Y");
		if (Input.GetKeyDown (KeyCode.C)) 
		{
			tempPosition = transform.position;
			tempRotation = transform.rotation;
			transform.position = oldPosition;
			transform.rotation = oldRotation;
			oldPosition = tempPosition;
			oldRotation = tempRotation;
			camera.orthographic = !camera.orthographic;
		}
		if (Input.GetKey (KeyCode.LeftShift)) 
		{
			shiftSpeed = 1.5f;
		}
		else
		{
			shiftSpeed = 1.0f;
		}
		if (!camera.orthographic) {
			if (Input.GetKey (KeyCode.E)) {
				moveUp = 1.0f;
			}
			if (Input.GetKey (KeyCode.Q)) {
				moveUp = -1.0f;
			}
			
			if (Input.GetMouseButton (1)) {
				transform.Rotate (-rotateVertical * rotateSpeed, rotateHorizontal * rotateSpeed, 0.0f);
			}
			transform.Translate (moveHorizontal * shiftSpeed, moveUp * shiftSpeed, moveVertical * shiftSpeed);
		} else 
		{
			transform.Translate (moveHorizontal * shiftSpeed, moveVertical * shiftSpeed, 0.0f);
		}
		

	}


}