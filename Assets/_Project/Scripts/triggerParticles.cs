using UnityEngine;
using System.Collections;

public class triggerParticles : MonoBehaviour {
	public ParticleEmitter fireTrap;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
//			if(fireTrap != null)
//				fireTrap.emit = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
//			if(fireTrap != null)
//				fireTrap.emit = false;
		}
	}
}
