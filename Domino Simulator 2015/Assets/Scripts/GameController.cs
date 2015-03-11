using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Linq;

public class GameController : MonoBehaviour {
	private CameraController mainCamera;
	int floorMask;
	public bool initial;
	private GameObject dominoToEdit;
	private List<dominoPos> toSave;
	private List<GameObject> currentOnBoard;
	public GameObject[] dominos = new GameObject[6];
	public float pushForce;
	public GUIText questionMark;
	public GUIText helpText;
	public GUIText cameraText;
	void Start()
	{
		cameraText.text = "Camera";
		questionMark.text = "?";
		helpText.text = "";
		currentOnBoard = new List<GameObject> ();
		toSave = new List<dominoPos>();
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
			if (!(Input.mousePosition.x > Screen.width - 300 && Input.mousePosition.y > Screen.height - 50)) {
				if (Input.GetButtonDown ("Fire1")) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hitPoint;
					
					if(Physics.Raycast(ray, out hitPoint, 200f, floorMask ))
					{
						Vector3 newHit = new Vector3(hitPoint.point.x, hitPoint.point.y + .5f, hitPoint.point.z);
						Quaternion rotate = Quaternion.identity;
						if(!Physics.CheckSphere(newHit,.27f))
						{
							GameObject toAdd = (GameObject)Instantiate(dominos[Random.Range(0,5)], newHit, rotate);
							dominoToEdit = toAdd;
							currentOnBoard.Add(toAdd);
						}
					}
				}
				if (Input.GetButtonUp ("Fire1")) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hitPoint;
					
					if(Physics.Raycast(ray, out hitPoint, 200f, floorMask ))
					{
						Vector3 newHit = new Vector3(hitPoint.point.x, hitPoint.point.y + .5f, hitPoint.point.z);
						
						Vector3 playerToMouse = newHit - dominoToEdit.transform.position;
						if(playerToMouse.magnitude > 0.05){
							Quaternion newRotation = Quaternion.LookRotation(playerToMouse) * Quaternion.Euler(0, -90, 0);
							dominoToEdit.rigidbody.MoveRotation(newRotation);
						} else {
							playerToMouse = new Vector3(0f,0f,0f);
						}
						dominoPos domPos = new dominoPos(
							dominoToEdit.transform.position.x,
							dominoToEdit.transform.position.y,
							dominoToEdit.transform.position.z,
							playerToMouse.x,
							playerToMouse.y,
							playerToMouse.z
							);
						toSave.Add(domPos);
					}
				}
				
				if (Input.GetButtonDown ("Fire2")) 
				{
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hitPoint;
					
					if(Physics.Raycast(ray, out hitPoint))
					{
						if (hitPoint.rigidbody.tag == "Domino"){
							Destroy(hitPoint.collider.gameObject);
						}
						
					}
				}
			}
		} 
		if (Input.GetKeyDown (KeyCode.P) && !mainCamera.isOrthographic) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				if (hit.rigidbody != null)
					hit.rigidbody.AddForceAtPosition(ray.direction * pushForce, hit.point);
		}
		if (Input.GetKeyDown (KeyCode.O) && !mainCamera.isOrthographic) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				if (hit.rigidbody != null)
					hit.rigidbody.AddExplosionForce(300.0f, hit.point, 25.0f);
		}
		if (Input.GetKeyDown (KeyCode.M)) 
		{
			saveGame ("test", toSave.ToArray());
		}
		if (Input.GetKeyDown (KeyCode.L)) 
		{
			loadGame("test");
		}
	}

	public void setHelpText() {
		if (helpText.text.Length <= 0) {
			helpText.text = "Left Click to place Domino\n" +
				"Release Left Click in wanted rotation direction\n" +
				"Right Click to Delete Domino\n" +
				"W A S D for horizontal movement\n" +
				"Q E for vertical movement\n" +
				"P to push\n" +
				"M to save\n" +
				"L to load previous save\n" +
				"C to swap Camera view\n" +
				"Hold Right click and drag for horizontal panning\n" +
				"Hold R and drag with mouse for vertical panning\n";
		} else {
			helpText.text = "";
		}
	}

	public void saveGame() {
		saveGame ("test", toSave.ToArray ());
	}

	public void saveGame(string filename, dominoPos[] toSave)
	{
		using (XmlWriter write = XmlWriter.Create(filename.Trim() + ".xml")) {
			write.WriteStartDocument ();
			write.WriteStartElement ("Dominos");
			foreach (dominoPos item in toSave) {
				write.WriteStartElement ("dominoPos");
				write.WriteStartElement("posx");
				write.WriteValue (item.posx);
				write.WriteEndElement();
				write.WriteStartElement("posy");
				write.WriteValue (item.posy);
				write.WriteEndElement();
				write.WriteStartElement("posz");
				write.WriteValue (item.posz);
				write.WriteEndElement();
				write.WriteStartElement("rotx");
				write.WriteValue (item.rotx);
				write.WriteEndElement();
				write.WriteStartElement("roty");
				write.WriteValue (item.roty);
				write.WriteEndElement();
				write.WriteStartElement("rotz");
				write.WriteValue (item.rotz);
				write.WriteEndElement();
				write.WriteEndElement();
			}
			write.WriteEndElement ();
			write.WriteEndDocument ();
		}
	}

	public void loadGame() {
		loadGame ("test");
	}

	public void loadGame(string filename)
	{
		var xmlStr = File.ReadAllText(filename.Trim ()+".xml");

		var str = XElement.Parse (xmlStr);

		var result = str.Elements("dominoPos").ToList();

		//need to clear the board of all dominos before loading the dominos from file
		foreach (GameObject domino in currentOnBoard) {
			Destroy(domino);
		}

		currentOnBoard = new List<GameObject> ();

		foreach (var dominoPos in result) {
			//place each domino on the board depending on posx, posy, posz, rotw, rotx, rotz
			float posx = float.Parse(dominoPos.Element("posx").Value);
			float posy = float.Parse(dominoPos.Element("posy").Value);
			float posz = float.Parse(dominoPos.Element("posz").Value);
			float rotx = float.Parse(dominoPos.Element("rotx").Value);
			float roty = float.Parse(dominoPos.Element("roty").Value);
			float rotz = float.Parse(dominoPos.Element("rotz").Value);

			Vector3 positionVetex = new Vector3(posx, posy, posz);
			Vector3 rotationQuat = new Vector3(rotx, roty, rotz);
			GameObject toAdd = (GameObject)Instantiate(dominos[Random.Range(0,5)], positionVetex, Quaternion.identity);
			if(rotationQuat.x !=0 || rotationQuat.y !=0 || rotationQuat.z!= 0){
				Quaternion newRotation = Quaternion.LookRotation(rotationQuat) * Quaternion.Euler(0, -90, 0);
				toAdd.rigidbody.MoveRotation(newRotation);
			}
			currentOnBoard.Add(toAdd);
			dominoPos domPos = new dominoPos(
				posx,
				posy,
				posz,
				rotx,
				roty,
				rotz
			);
			toSave.Add (domPos);
		}
	}

	/*void loadGame(string filename)
	{
		using (XmlReader read = XmlReader.Create (filename + ".xml")) {
			while(read.Read ())
			{
				if(read.IsStartElement())
				{
					while(read.has)
					{

					}
				}
			}
		}

	}*/

	public class dominoPos{

		public float posx;
		public float posy;
		public float posz;
		public float rotx;
		public float roty;
		public float rotz;

		public dominoPos(float posx, float posy, float posz, float rotx, float roty, float rotz)
		{
			this.posx = posx;
			this.posy = posy;
			this.posz = posz;
			this.rotx = rotx;
			this.roty = roty;
			this.rotz = rotz;
		}
	}
}
