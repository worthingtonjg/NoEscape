using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class triggerScript : MonoBehaviour {
	private doorController doors;

	// Use this for initialization
	void Start () {
		var scripts = GameObject.Find ("_scripts");
		doors = scripts.GetComponent<doorController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player" && !timeController.fadingOut)
		{
			int triggerIndex = int.Parse(this.name.Substring(this.name.Length-3));

			if(StoryManager.Stories[triggerIndex].Active) return;

			bool activated = StoryManager.Stories[triggerIndex].ActivateIfCan();

			if(activated)
			{
				Debug.Log(triggerIndex);

				foreach(int i in StoryManager.Stories[triggerIndex].DoorsToOpenOnActive)
				{
					doors.OpenCellDoor(i);
				}

				foreach(int i in StoryManager.Stories[triggerIndex].DoorsToCloseOnActive)
				{
					doors.CloseCellDoor(i);
				}
			}
		}
	}
}
