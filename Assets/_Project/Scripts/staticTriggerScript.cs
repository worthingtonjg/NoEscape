using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class staticTriggerScript : MonoBehaviour {
	private bool showStatic;
	private static int triggerIndex;
	private static List<StoryItem> _statics;
	
	public static List<StoryItem> Statics
	{
		get
		{
			if(_statics == null)
			{
				_statics = new List<StoryItem>();


				_statics.Add(null);
				_statics.Add(null);
				_statics.Add(null);
				_statics.Add(null);

				// 004
				_statics.Add (new StoryItem
				              {
					Texts = new string[] {"<color='orange'>[Scratching followed by hysterical laughing]</color>"},
				});

				// 005
				_statics.Add (new StoryItem
				              {
					Texts = new string[] {"<color='orange'>[Muffled conversation]</color>"},
				});
			}
			
			return _statics;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.fontSize = 18;
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		
		if(showStatic && triggerIndex < Statics.Count && Statics[triggerIndex] != null)
		{
			if(StoryManager.CurrentStory == null)
			{
				Debug.Log (triggerIndex);
				GUI.Box (new Rect(10, 10, 400, 300), Statics[triggerIndex].Texts[0]);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			triggerIndex = int.Parse(this.name.Substring(this.name.Length-3));
			Debug.Log(triggerIndex);

			showStatic = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			showStatic = false;
		}
	}
}
