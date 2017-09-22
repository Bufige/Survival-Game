using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CursorManager : MonoBehaviour {

	public PauseManager pauseManager;
	public GameObject player;
	private RigidbodyFirstPersonController rigidbodyFirstPersonController;
	// Use this for initialization
	void Start () {
		pauseManager = GameObject.FindGameObjectWithTag ("PauseManager").GetComponent<PauseManager>();	 // find pause manager
		rigidbodyFirstPersonController = player.GetComponent<RigidbodyFirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {

		if (pauseManager.gameState == PauseManager.GameState.Playing) {
			HideLockCursor ();
			rigidbodyFirstPersonController.enabled = true;
		} else if (pauseManager.gameState == PauseManager.GameState.Paused) {
			ShowUnlockCurso ();
		} else if (pauseManager.gameState == PauseManager.GameState.Death) {
			ShowUnlockCurso ();
		} else if (pauseManager.gameState == PauseManager.GameState.AcessingInventory) {
			ShowUnlockCurso ();
			rigidbodyFirstPersonController.enabled = false;
		}
	}
	private void HideLockCursor()
	{
		Cursor.visible = false; //hide cursor.
		Cursor.lockState = CursorLockMode.Locked; // lock the cursor.
	}
	private void ShowUnlockCurso()
	{
		Cursor.visible = true; // show cursor
		Cursor.lockState = CursorLockMode.None; // unlock cursor
 	}
}
