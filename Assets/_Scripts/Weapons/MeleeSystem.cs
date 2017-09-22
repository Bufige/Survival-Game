using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSystem : MonoBehaviour {

	[Header("Debug")]
	public bool debugMode;

	[Header("Configuraion")]
	public GameObject meleeObject;
	public int minDamage = 25;
	public int maxDamage = 50;
	public float weaponRange = 3.5f;
	public Camera FPSCamera;




	private TreeHealth treeHealth;
	private AdvancedEnemyAI enemyAI;

	private PauseManager pauseManager;
	// Use this for initialization
	void Start () {
		//newSwayPosition = meleeObject.transform.localRotation;
		pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (pauseManager.gameState != PauseManager.GameState.Playing) {
			return;
		}

		Ray ray = FPSCamera.ScreenPointToRay(new Vector2(Screen.width/2,Screen.height/2)); // Set ray to center of screen.
		RaycastHit hitInfo;
		if(debugMode)
			Debug.DrawRay (ray.origin,ray.direction * weaponRange,Color.blue);

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if(Physics.Raycast(ray,out hitInfo,weaponRange)) // if the ray hit.
			{

				//Here all the objects we can hit.

				if (hitInfo.collider.tag == "Tree") {

					treeHealth = hitInfo.collider.GetComponentInParent<TreeHealth> ();
					AttackTree ();
				}
				else if(hitInfo.collider.tag == "Enemy")
				{
					enemyAI = hitInfo.collider.GetComponent<AdvancedEnemyAI> ();
					int damage = Random.Range (minDamage,maxDamage);
					enemyAI.TakeDamage(damage);
					AttackEnemy ();
				}
			}
		}
	}

	private void FixedUpdate()
	{
		
	}


	private void AttackTree()
	{
		if (debugMode)
			Debug.Log ("Tree name :" + treeHealth.name + " health: " + treeHealth.health );
		
		int damage = Random.Range (minDamage,maxDamage); // Random damage.
		treeHealth.health -= damage;
	}

	private void AttackEnemy()
	{
		int damage = Random.Range (minDamage,maxDamage); // Random damage.
		enemyAI.TakeDamage(damage);
	}
}
