using UnityEngine;
using System.Collections;

public class timeController : MonoBehaviour 
{
	public static bool fadingOut;
	public static bool fadingIn;

	public static bool fading
	{
		get { return fadingIn || fadingOut; }
	}

	public float fadeSpeed = .10f; 

	public enum EnumTime
	{
		Day,
		Night
	};

	public EnumTime TimeOfDay = EnumTime.Day;
	public Light LightDay;
	public Light LightNight;
	public AudioClip AudioDay;
	public AudioClip AudioNight;
	public AudioSource Audio;
	public GUITexture gt;
	public GameObject SleepingMax;

	void Awake()
	{
		gt.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		fadingOut = false;
		fadingIn = false;
	}

	// Use this for initialization
	void Start () 
	{
		gt.color = Color.black;
		LightDay.enabled = false;
		LightNight.enabled = false;
		Debug.Log ("Time Controller Start: " + gt.color);
	}
	
	// Update is called once per frame
	void Update () {

		if(!fading)
		{
			if (TimeOfDay == EnumTime.Day && !LightDay.enabled) 
			{

				StartFade ();

			}

			if(TimeOfDay == EnumTime.Night && !LightNight.enabled)
			{
				StartFade();
			}
		}

		Fade();
	}

	void StartFade()
	{
		fadingOut = true;
		fadingIn = false;
	}

	void Fade ()
	{
		if(fadingOut)
		{
			if(gt.color.a < 0.5f)
			{
				// Lerp the colour of the texture between itself and black.
				gt.color = Color.Lerp(gt.color, Color.black, fadeSpeed * Time.deltaTime);
			}
			else
			{
				fadingOut = false;
				fadingIn = true;
				gt.color = Color.black;

				if (TimeOfDay == EnumTime.Day) 
				{
					SleepingMax.SetActive(false);
					LightDay.enabled = true;
					LightNight.enabled = false;
					Audio.clip = AudioDay;
					Audio.Play();


				}
				
				if(TimeOfDay == EnumTime.Night)
				{
					SleepingMax.SetActive(true);
					LightDay.enabled = false;
					LightNight.enabled = true;
					Audio.clip = AudioNight;
					Audio.Play();

				}
			}
		}

		if(fadingIn)
		{
			if(gt.color.a > 0.001f)
			{
				gt.color = Color.Lerp(gt.color, Color.clear, fadeSpeed * Time.deltaTime);
			}
			else
			{
				gt.color = Color.clear;
				fadingIn  = false;


			}
		}
	}
}
