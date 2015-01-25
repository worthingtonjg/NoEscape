using UnityEngine;
using System.Collections;

public class scene3Exit : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Application.LoadLevel("Scene04");
		}
	}
}
