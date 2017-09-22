using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {


	public Button startGame,exitGame;
	public RawImage menuBackground;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame()
	{
		//startGame.enabled = false;
		//exitGame.enabled = false;
		//menuBackground.enabled = false;
		SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
		//SceneManager.LoadScene ("Game", LoadSceneMode.Additive);
	}

	public void ExitGame()
	{		
		Debug.Log ("Exiting Game...");
		Application.Quit ();
	}


}
