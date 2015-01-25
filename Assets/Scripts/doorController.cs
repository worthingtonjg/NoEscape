using UnityEngine;
using System.Collections;

public class doorController : MonoBehaviour {
	public GameObject[] openDoors;
	public GameObject[] closedDoors;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenCellDoor(int cell)
	{
		if(openDoors[cell] != null)
			openDoors [cell].SetActive (true);

		if(closedDoors[cell] != null)
			closedDoors [cell].SetActive (false);
	}

	public void CloseCellDoor(int cell)
	{
		if(openDoors[cell] != null)
			openDoors [cell].SetActive (false);

		if(closedDoors[cell] != null)
			closedDoors [cell].SetActive (true);
	}
}
