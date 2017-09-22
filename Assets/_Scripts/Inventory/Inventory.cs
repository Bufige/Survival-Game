using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour {

	[Header("Configuration")]
	public KeyCode keyCode = KeyCode.Tab;
	public int slotAmount = 10;
	public GameObject slotPrefab;
	public GameObject slotParent;
	public Canvas inventoryCanvas;
	public bool isShowing;

	private PauseManager pauseManager;

	// Use this for initialization
	void Start () {
		CreateSlots (slotAmount);
		pauseManager = GameObject.FindGameObjectWithTag ("PauseManager").GetComponent<PauseManager>();
		inventoryCanvas.enabled = false;
		isShowing = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (pauseManager.gameState == PauseManager.GameState.Paused || pauseManager.gameState == PauseManager.GameState.Death ) {
			isShowing = false;
			inventoryCanvas.enabled = false;
			return;
		}

		if (Input.GetKeyDown (keyCode)) {
			isShowing = !isShowing;
		}
		if (isShowing == true) {
			pauseManager.gameState = PauseManager.GameState.AcessingInventory;
			inventoryCanvas.enabled = true;
		} else {
			if (pauseManager.gameState == PauseManager.GameState.AcessingInventory) {
				pauseManager.gameState = PauseManager.GameState.Playing;
			}
			inventoryCanvas.enabled = false;
		}
	}
	private void CreateSlots(int slots)
	{
		for (int i = 0; i < slots; i++) {
			GameObject slotInstance = (GameObject)Instantiate (slotPrefab);
			slotInstance.transform.SetParent (slotParent.transform);
		}
	}
}
