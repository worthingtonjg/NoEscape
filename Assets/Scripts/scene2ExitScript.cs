using UnityEngine;
using System.Collections;

public class scene2ExitScript : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Application.LoadLevel("Scene03");
		}
	}
}
