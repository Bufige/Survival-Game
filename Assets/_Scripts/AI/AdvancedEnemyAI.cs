using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdvancedEnemyAI : MonoBehaviour {


	[Header("Debug")]
	private Ray ray;
	private RaycastHit hitInfo;
	public bool debugMode;

	[Header("Configuration")]
	public int health = 100;
	public float viewRange = 25f;
	public float attackRange = 5f;
	public float eyeHeight;
	public float attackTime = 1f;


	public float randomUnityCircleRadius = 25f;
	public bool isChasing = false;
	public bool isAttacking = false;
	public float distanceToPlayer;


	public Transform playerTransform;
	public Transform playerTransformDist;



	[Header("Damage")]
	public float minimumDamage = 25f;
	public float maximumDamage = 40f;

	[Header("Think")]
	public float thinkTimer = 5f;
	private float thinkTimerMin = 5f;
	private float thinkTimerMax = 15f;






	private NavMeshAgent agent;
	private float attackTimeStart;
	private Player player;
	private Utility utility;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		utility = GameObject.FindGameObjectWithTag("Utility").GetComponent<Utility>();
		InvokeRepeating ("GetPlayer", 0f, 5f);
		attackTimeStart = attackTime;
		//thinkTimer = thinkTimerStart;
	}

	
	// Update is called once per frame
	void Update () {
		if(playerTransformDist != null)
			distanceToPlayer = Vector3.Distance (transform.position, playerTransformDist.position); // get player distance;


		Vector3 eyePosition = new Vector3 (transform.position.x,transform.position.y + eyeHeight,transform.position.z); // Eye position;

		ray = new Ray (eyePosition, transform.forward); // Eye position ray.

		CheckHealth (); // Check for enemy Health.

		thinkTimer -= Time.deltaTime; // We subtract thinktime over each update.

		if (distanceToPlayer <= attackRange && isChasing == true) {
			isAttacking = true;
		}
		if (isAttacking == true) {
			attackTime -= Time.deltaTime;
			isChasing = false;
			if(agent != null && health > 0)
				agent.SetDestination (transform.position); // reset position;
		}
		if (attackTime <= 0) {
			Attack ();
			attackTime = attackTimeStart;
			isAttacking = false;
			CheckForPlayer ();
		}

		if (thinkTimer <= 0) {
			Think ();
			thinkTimer = Random.Range(thinkTimerMin,thinkTimerMax);
		}

		CheckForPlayer ();


		if (isChasing) {
			if(health > 0 && playerTransform != null && agent != null)
				agent.SetDestination (playerTransform.position);
		}


		if (debugMode) {
			Debug.DrawRay (ray.origin,ray.direction*viewRange,Color.red);
		}
	}
	public void TakeDamage(int damage)
	{
		isChasing = true;

		playerTransform = player.transform;
		

		health -= damage;

		if (debugMode)
			Debug.Log (gameObject.name + " health: " + health);
	}

	private void CheckHealth()
	{
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

	private void Think()
	{
		//Think if not chasing.
		if (isChasing == false) {
			Vector3 newPos = transform.position + new Vector3 (Random.insideUnitCircle.x * randomUnityCircleRadius,Random.insideUnitCircle.y,Random.insideUnitCircle.y * randomUnityCircleRadius);
			if(agent != null)
				agent.SetDestination (newPos);			
		}
	}
	private void Attack()
	{
		if (Physics.Raycast (ray,out hitInfo,attackRange)) { // Check if in range of raycast.
			if (hitInfo.collider.tag == "Player") {
				var damage = Random.Range(minimumDamage,maximumDamage); // Set random damage;
				player.AttackPlayer (damage);
			}
		}
	}

	private void CheckForPlayer()
	{
		if (Physics.Raycast (ray, out hitInfo,viewRange)) {
			if (hitInfo.collider.tag == "Player") {
				if (isChasing == false && isAttacking == false) {
					
					isChasing = true; // Set chasing true

					if (playerTransform == null) {

						playerTransform = hitInfo.collider.GetComponent<Transform> ();
					}
				}
			}
		}
	}

	private void GetPlayer()
	{
		//Here we find the closest player
		playerTransformDist = utility.FindClosestPlayer (gameObject).transform;
		player = playerTransformDist.GetComponent<Player> ();
	}

}
