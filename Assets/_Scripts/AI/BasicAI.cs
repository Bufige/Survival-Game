using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour {


	public int health = 100;
	private NavMeshAgent agent;

	private Transform playerTransform;
	private bool isChasing = false;


	public bool debugMode;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isChasing) {
			agent.SetDestination (playerTransform.position);
		}

		if (health <= 0) {
			Destroy (gameObject);
		}
	}

	private void OnTriggerEnter(Collider target)
	{		
		if(target.tag == "Player")
		{
			isChasing = true;
			playerTransform = target.transform;
			agent.SetDestination (playerTransform.position);
		}
	}


	public void TakeDamage(int damage)
	{
		health -= damage;
		if (debugMode)
			Debug.Log (gameObject.name + " health: " + health);
	}
}
