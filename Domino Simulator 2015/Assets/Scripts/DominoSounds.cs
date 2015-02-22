using UnityEngine;
using System.Collections;

public class DominoSounds : MonoBehaviour {

	public AudioSource[] src = new AudioSource[2];

	void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody != null) {
			if (collision.rigidbody.tag == "Domino") {
				src[0].Play();
			}
		} else {
			src[1].Play ();
		}
	}

}
