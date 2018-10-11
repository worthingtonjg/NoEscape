using UnityEngine;
using System.Collections;

public class menuScript : MonoBehaviour {
	private int windowWidth = 400;
	private int windowHeight = 288;
	private int buttonMargin=30;
	private int buttonHeight = 70;
	private int margin = 90;
	private bool startingGame;
	private bool showCredits;
	private bool guiEnabled;

	public Texture2D Title;
	public Texture2D StartButton;
	public Texture2D CreditsButton;
	public Texture2D ExitButton;
	public GUITexture Fader;
	public GUIText[] guiText;

	public float fadeSpeed = 1f;

	void Start()
	{
	}

	void Update () 
	{
		if (startingGame)
			StartGame ();
	}

	void OnGUI()
	{
		//if (!Credits.showCredits) {
		if (Fader.color.a >= .10)
			return;

		if(!guiEnabled)
		{
			foreach(var gui in guiText)
			{
				gui.enabled = true;
			}

			guiEnabled = true;
		}

		if (GUI.Button (new Rect ((Screen.width - windowWidth) / 2, (Screen.height - windowHeight) / 2, windowWidth-buttonMargin, buttonHeight), StartButton)) {
			startingGame = true;

			foreach(var gui in guiText)
			{
				gui.enabled = false;
			}
		}

		//if (GUI.Button (new Rect ((Screen.width - windowWidth) / 2, ((Screen.height - windowHeight) / 2)+margin, windowWidth-buttonMargin, buttonHeight), CreditsButton)) {
		
		//}

		if (GUI.Button (new Rect ((Screen.width - windowWidth) / 2, ((Screen.height - windowHeight) / 2)+(margin), windowWidth-buttonMargin, buttonHeight), ExitButton)) {
			Application.Quit();
		}
	}

	void StartGame ()
	{
		// Lerp the colour of the texture between itself and black.
		Fader.color = Color.Lerp(Fader.color, Color.black, fadeSpeed * Time.deltaTime);

		if (Fader.color.a < 0.6f)
			return;

		startingGame = false;
		Application.LoadLevel ("Scene01");
	}
}
