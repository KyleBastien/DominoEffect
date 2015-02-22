using UnityEngine;
using System.Collections;

public class RandomFaces : MonoBehaviour {
	public GameObject Right;
	void Start(){
		Material[] mat = new Material[6];
		mat = (Material[])Resources.LoadAll ("Materials/Domino Faces");

		Right.renderer.material = mat[Random.Range(0,6)];
	}
}
