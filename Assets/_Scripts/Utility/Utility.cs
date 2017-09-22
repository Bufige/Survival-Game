using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Utility : MonoBehaviour{


	public bool debugMode;

	public string screnShotPath = "/Screenshots/";
	public string screenShotName = "Screenshot";
	private GameObject[] gameObjects;



	private void Start()
	{
		gameObjects = GameObject.FindGameObjectsWithTag ("Player");
	}

	public  GameObject FindClosestPlayer(GameObject obj)	{

		// we create a nulll player.
		GameObject player = null;

		//we check if we have any player obj;
		if (gameObjects.Length > 0) {

			//set distance to max distance;
			var distance = float.MaxValue;

			foreach (var i in gameObjects) { // loop all players
				//difference of position
				var diff = Vector3.Distance (obj.transform.position, i.transform.position);
				if (distance > diff) {
					//we found the closest;
					distance = diff;
					player = i;
				}
			}
		}
		return player; // return the closest
	}
	public  GameObject[] FindPlayers()
	{
		return gameObjects;
	}
	private string GetPath()
	{
		string path = Application.dataPath;
		int last = path.LastIndexOf ('/');
		path = path.Substring(0,last);
		return path + screnShotPath;
	}
	public void TakeScreenshot()
	{

		if (!Directory.Exists (GetPath())) { // check if the folder exists;
			if (debugMode) {
				Debug.Log ("path do not exist");
				Debug.Log (GetPath());
			}
			Directory.CreateDirectory (GetPath());
			Application.CaptureScreenshot (GetPath() + screenShotName + "1.png");
		} else { // Folder exists
			if (debugMode) {
				Debug.Log ("path exist,create new file");
				Debug.Log (GetPath());
			}
			var files = Directory.GetFiles (GetPath()).Where(x=> x.IndexOf(screenShotName) != 1 && x.IndexOf(".png") != -1);

			if (files.Count () == 0) {				
				Application.CaptureScreenshot ( (GetPath() + screenShotName) + "1.png");
			} else {
				string currentSS = files.Last ().Remove (0, (GetPath() + screenShotName).Count());

				if (debugMode) 
				{
					Debug.Log ("Path0:" + GetPath());
					Debug.Log ("Path1: " + currentSS);
				}
				int size = currentSS.Count ();
				size--;
				currentSS = currentSS.Remove (size - 3, size);
				int nextSS = int.Parse (currentSS);
				if(debugMode)
					Debug.Log ("Path2 : " + (GetPath() + screenShotName) + ((nextSS + 1) + ".png"));

				Application.CaptureScreenshot ( (GetPath() + screenShotName) + ((nextSS + 1) + ".png"));				
			}
		}
	}
}
