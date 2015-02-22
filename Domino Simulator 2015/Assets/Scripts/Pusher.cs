using UnityEngine;
using System.Collections;

public class Pusher : MonoBehaviour {

	public float pokeForce;
	void Update() {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				if (hit.rigidbody != null)
					hit.rigidbody.AddForceAtPosition(ray.direction * pokeForce, hit.point);
	}
}
