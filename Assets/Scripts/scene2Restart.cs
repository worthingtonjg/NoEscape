using UnityEngine;
using System.Collections;

public class scene2Restart : MonoBehaviour 
{
	public ParticleEmitter fireTrap;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			var respawn = GameObject.FindGameObjectWithTag("Respawn");

			if(fireTrap != null)
				fireTrap.emit = false;

			other.transform.position = respawn.transform.position;
		}
	}
}
