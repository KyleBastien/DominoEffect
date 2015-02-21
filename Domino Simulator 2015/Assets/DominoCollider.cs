using UnityEngine;
using System.Collections;

public class DominoCollider : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Domino") 
		{
			Destroy (gameObject);
		}
			
	}
}
