using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class gameController : MonoBehaviour {

	private timeController timeController;
	private GameObject player;
	private GameObject spawner;
	private GameObject location1;

	public List<AudioClip> Clips;
	public List<GameObject> Focusable;
	public ParticleEmitter gas;

	// Use this for initialization
	void Start () 
	{
		timeController = this.GetComponentInParent<timeController>();
		player = GameObject.FindGameObjectWithTag ("Player");
		spawner = GameObject.FindGameObjectWithTag ("Respawn");
		location1 = GameObject.FindGameObjectWithTag("relocate1");

		SetupAudioClips();
		SetupFocusableItems();
	}

	private void SetupFocusableItems()
	{
		StoryManager.Stories [1].FocusOn = Focusable [0];  // Cell door
		StoryManager.Stories [2].FocusOn = Focusable [1];  // Max's Bed
		StoryManager.Stories [3].FocusOn = Focusable [1];  // Max's Bed
		StoryManager.Stories [5].FocusOn = Focusable [2];  // Guard Room Door
		StoryManager.Stories [4].FocusOn = Focusable [3];  // Crazy digger prisoner door
		StoryManager.Stories [6].FocusOn = Focusable [4];

	}

	private void SetupAudioClips()
	{
		StoryManager.Stories [0].Clips = new AudioClip[] { Clips [0] };  // Max1
		StoryManager.Stories [1].Clips = new AudioClip[] { Clips [1] };  // Max2
		StoryManager.Stories [2].Clips = new AudioClip[] { Clips [2] };  // Max3
		StoryManager.Stories [3].Clips = new AudioClip[] { Clips [3] };  // Max4
		StoryManager.Stories [4].Clips = new AudioClip[] { Clips [8] };  // Prisoner1_1
		StoryManager.Stories [5].Clips = new AudioClip[] { 
			Clips[4],  // Guard1_1
			Clips[5],  // Guard2_1
			Clips[6],  // Guard1_2
			Clips[7],  // Guard2_2
		};  
		StoryManager.Stories [6].Clips = new AudioClip[] { Clips [9] };  // Narrator1
		StoryManager.Stories [8].Clips = new AudioClip[] { Clips [10] };  // Narrator1
		StoryManager.Stories [9].Clips = new AudioClip[] { 
			Clips[12],  // Narrator2
			Clips[13],  // Narrator3
			Clips[14],  // Narrator4
		};  
		StoryManager.Stories [10].Clips = new AudioClip[] { 
			Clips[15],  // Narrator5
			Clips[11],  // prisoner2_1
			Clips[16],  // Narrator6
			Clips[17],  // Guard1 - We warned you
			Clips[18],  // Dispose of his body
			Clips[19],  // We are transferring
			Clips[20],  // You feel yourself getting dizzy
		};  
		
		StoryManager.Stories [11].Clips = new AudioClip[] { Clips [21] };  // Narrator1 - You must have passed out
		StoryManager.Stories [12].Clips = new AudioClip[] { 
			Clips [22],
			Clips [23],
		};  
		
		StoryManager.Stories [14].Clips = new AudioClip[] { Clips [24] };  // Narrator1 - You must have passed out
		StoryManager.Stories [15].Clips = new AudioClip[] { Clips [25] };  // Guard - Ready, despensing gas
		StoryManager.Stories [16].Clips = new AudioClip[] { Clips [26] };  // Max - Seriously
		
		StoryManager.Stories [17].Clips = new AudioClip[] { 
			Clips [27],  // That was different
			Clips [28],  // Am I awake
		};
		
		StoryManager.Stories [18].Clips = new AudioClip[] { Clips [29] };  // The Trap door is real
	}

	// Update is called once per frame
	void Update () {
		var storyItem = StoryManager.CurrentStory;

		if(Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonDown(0))
		{
			if(storyItem != null)
			{
				if(player.GetComponent<AudioSource>().isPlaying) {
					player.GetComponent<AudioSource>().Stop();
				}

				bool foundNext = storyItem.MoveNext();

				if(storyItem.Recieved)
				{
					ToggleControls(true);

					if(storyItem.SleepOnReceived)
					{
						timeController.TimeOfDay = timeController.EnumTime.Night;
					}

					if(storyItem.AwakeOnReceived)
					{
						timeController.TimeOfDay = timeController.EnumTime.Day;
					}

					if(storyItem.RespawnOnReceived)
					{
						player.transform.position = spawner.transform.position;
					}

					if(storyItem.RelocateToLocation1OnReceived)
					{
						player.transform.position = location1.transform.position;
					}

					if(storyItem.EmitGas)
					{
						//gas.emit = true;
					}
					else
					{
						//gas.emit = false;
					}

					if(storyItem.NextScenOnReceive)
					{
						Application.LoadLevel("Scene02");
					}
				}
			}
		}
	}

	void OnGUI()
	{
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.fontSize = 18;
		GUI.skin.box.alignment = TextAnchor.MiddleCenter;

		var currentStory = StoryManager.CurrentStory;

		if(currentStory != null)
		{
			ToggleControls(false);

			AudioClip clip = currentStory.GetCurrentClip();

			if(clip != null && !currentStory.GetClipPlayed() )
			{
				var source = player.GetComponent<AudioSource>();

				source.clip = clip;
				source.Play();
				currentStory.SetClipPlayed();

				if(currentStory.FocusOn != null)
				{
					// Not currently working
					//player.transform.LookAt(new Vector3(currentStory.FocusOn.transform.position.x, currentStory.FocusOn.transform.position.y, currentStory.FocusOn.transform.position.y));
				}
			}




			GUI.Box (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300), currentStory.GetCurrent()+"\n\n<color='orange'>(Click to Continue)</color>");
		}
	}

	private void ToggleControls(bool enabled)
	{
		player.SendMessage("ToggleControl", enabled);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = enabled;
		player.GetComponent<MouseLook>().enabled = enabled;
	}
}

public static class StoryManager
{
	private static List<StoryItem> _stories;

	public static List<StoryItem> Stories 
	{ 
		get 
		{
			if(_stories == null)
			{
				_stories = new List<StoryItem> ();

				// 000
				_stories.Add (new StoryItem 
				{
					Texts = new string[] {"Where am I?  This looks like ... a prison cell!\n\nHow did I get here?  What do I do now?"}, 

					Active = true 
				});

				// 001
				_stories.Add (new StoryItem 
				{
					Texts = new string[] {"Guards... Hello... Anybody?"},
					PreReqs = new int[] {0}
				});

				// 002
				_stories.Add (new StoryItem
				{
					Texts = new string[] {"My head hurts... I'm so tired... <color='green'>(Yawn)</color>\n\nI can't stay awake...\n\n <color='green'>(Drifts into sleep)</color>"},
					PreReqs = new int[] {1},
					SleepOnReceived = true
				});

				// 003
				_stories.Add(new StoryItem
				{
					Texts = new string[] {"Holy ... Is that my body?\n\nAm I ... Dead?"},
					PreReqs = new int[] {2},
					DoorsToOpenOnActive = new int[] { 0,1 }
				});

				// 004
				_stories.Add(new StoryItem
				{
					Texts = new string[] {"Scratch... Scratch... Scratch...\n\n<color='orange'>From Inside the cell:</color> Just a little further... and I will finally be free!"},
					PreReqs = new int[] {3},
				});

				// 005
				_stories.Add(new StoryItem
				{
					Texts = new string[] {"<color='orange'>Guard #1:</color> Did you hear about the guy in cell 2?", "<color='orange'>Guard #2:</color> The crazy guy?", "<color='orange'>Guard #1:</color>Yeah, We are moving him to solitary in the morning.  He's caused way too much trouble lately.", "<color='orange'>Guard #2:</color> The sooner the better, he gives me the creeps."},
					PreReqs = new int[] {3},
				});

				// 006
				_stories.Add (new StoryItem
				              {
					Texts = new string[] {"<color='orange'>[You hear moaning from all around]</color>\n\nYou notice that the wall between this cell and yours is thin and crumbling on this side.\n\n<color='orange'>[You feel a presence in the room - as if someone is watching you.]</color>"},
					PreReqs = new int[] {3}
				});

				// 007
				_stories.Add(new StoryItem
                {
					Texts = new string[] {"<color='orange'>[Your vision fades]...</color>"},
					PreReqs = new int[] {4,5,6},
					AwakeOnReceived = true,
					DoorsToCloseOnActive = new int[] { 0, 1 }
				});

				// 008
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"Uh... That was strange.\n\nMust have been a dream?"},
					PreReqs = new int[] {7},
				});

				// 009
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {
						"You hear moaning coming from the other side of the wall.",
						"Remebering how thin the wall looked you wonder if you could break through?",
						"You give the wall a kick..."},
					PreReqs = new int[] {8},
				});

				// 010
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {
						"The wall crumbles!\n\n<color='orange'>[The moaning from the other side changes to a shriek of terror!]</color>",
						"<color='orange'>Prisoner Screaming:</color> No! No!  They will kill me!  You have killed me!  YOU HAVE KILLED ME!",
						"<color='orange'>[Shouting and footsteps in the hall]</color>\n\nThe prisoner's cry escalates as his cell door is slammed open.",
						"<color='orange'>Guard #1:</color> We warned you about trying to escape!\n\n<color='orange'>[Grunts of pain are heard, followed by a final shriek]</color>\n\nThen silence...",
						"<color='orange'>Guard #1:</color> Dispose of his body.  And this hole in the wall will need to be repaired.",
						"<color='orange'>Guard #2:</color> We are transfering the prisoner in cell #2, we can move the prisoner in cell #1 to that cell until then.\n\n<color='orange'>[Grunts are heard as the body is dragged away.]</color>",
						"You feel your self getting dizzy."},
					PreReqs = new int[] {9},
					DoorsToOpenOnActive = new int[] {4},
					DoorsToCloseOnActive = new int[] {2,3},
					SleepOnReceived = true
				});

				// 011 
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"You must have passed out...\n\nConfused you look around...\n\nWhy does this keep happening?"},
					PreReqs = new int[] {10},
					DoorsToOpenOnActive = new int[] {0,5,6},
				});

				// 012
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"As you enter the room the sound of scratching and hysterical laughter gets louder.",
						"Unsure where the sound is coming from you ignore it.\n\nIn the corner you see a dark passage."},
					DoorsToCloseOnActive = new int[] {0,5}
				});

				// 013
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"Your vision fades"},
					AwakeOnReceived = true,
					RespawnOnReceived = true,

				});

				// 014
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"These visions are starting to piss me off.\n\nI wonder if that passage is real?"},
					PreReqs = new int[] { 13 },
					DoorsToCloseOnActive = new int[] {6}
				});


				// 015
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"[You here voices in the hall]\n\nReady to relocate prisoner to cell #2...Dispensing Gas"},
					PreReqs = new int[] { 14 },
					EmitGas = true
				});

				// 016	
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"Seriously?"},
					PreReqs = new int[] { 15 },
					SleepOnReceived = true,
					RelocateToLocation1OnReceived = true
				});

				// 017		
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"Okay, that was different?", "Am I awake?"},
					PreReqs = new int[] { 16 },
					AwakeOnReceived = true
				});

				// 018	
				_stories.Add(new StoryItem
				             {
					Texts = new string[] {"I knew it ... The trap door is real!"},
					PreReqs = new int[] { 17 },
					DoorsToOpenOnActive = new int[] {6},
					NextScenOnReceive = true
				});
				
				
			}

			return _stories;
		}
	}

	public static StoryItem CurrentStory { get { return Stories.FirstOrDefault (s => !s.Recieved && s.Active); } }

}

public class StoryItem
{
	private AudioClip[] _clips;

	private int textIndex { get; set; }
	public AudioClip[] Clips 
	{ 
		get { return _clips; }
		set 
		{
			_clips = value;
			ClipPlayed = new bool[_clips.Length];
		} 
	}

	public string[] Texts { get; set; }

	public bool Recieved { get { return textIndex >= Texts.Length; } }

	public bool Active { get; set; }

	public int[] PreReqs { get; set; }

	public bool SleepOnReceived { get; set; }

	public bool AwakeOnReceived { get; set; }

	public int[] DoorsToOpenOnActive { get; set; }

	public int[] DoorsToCloseOnActive { get; set; }

	public bool RespawnOnReceived { get; set; }

	public bool RelocateToLocation1OnReceived { get; set; }

	public bool EmitGas { get; set; }

	public bool NextScenOnReceive { get; set; }

	public bool[] ClipPlayed { get; set; }

	public GameObject FocusOn { get; set; }

	public StoryItem()
	{
		Clips = new AudioClip[] {};
		ClipPlayed = new bool[] {};
		Texts = new string[] {};
		PreReqs = new int[] {};
		DoorsToOpenOnActive = new int[] {};
		DoorsToCloseOnActive = new int[] {};
	}

	public string GetCurrent()
	{
		string result = null;

		if(textIndex < Texts.Length)
		{
			result = Texts[textIndex];
		}

		return result;
	}

	public AudioClip GetCurrentClip()
	{
		AudioClip result = null;

		if(textIndex < Clips.Length)
		{
			result = Clips[textIndex];
		}

		return result;
	}

	public bool GetClipPlayed()
	{
		bool result = false;
		if(textIndex < ClipPlayed.Length)
		{
			result = ClipPlayed[textIndex];
		}

		return result;
	}

	public void SetClipPlayed()
	{
		if(textIndex < ClipPlayed.Length)
		{
			ClipPlayed[textIndex] = true;
		}
	}

	public bool MoveNext()
	{
		if(textIndex < Texts.Length)
		{
			++textIndex;
			return true;
		}

		return false;
	}

	public bool ActivateIfCan()
	{
		foreach(int i in PreReqs)
		{
			if(!StoryManager.Stories[i].Recieved) return false;
		}

		Active = true;

		return true;
	}
}
