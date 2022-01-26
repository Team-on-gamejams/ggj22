using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.Weapons;
using BattleSystem.Health;

public class emeny_AI : MonoBehaviour {
	public BaseWeapon weapon;
	public Health health;

	public event Action EnemyAttack;
	public event Action Patrooling;
	public event Action Chasing;

	public Vector3 agent;

	public Transform player;

	public LayerMask whatIsGround, whatIsPlayer;

	public float speed;

	//Patroling
	public Vector3 walkPoint;
	bool walkPointSet;
	public float walkPointRange;

	//Attacking
	bool alreadyAttacked;

	//States
	public float sightRange, attackRange;
	public bool playerInSightRange, playerInAttackRange;




	private void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update() {
		//Check for sight and attack range
		//playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
		//playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

		if (Vector3.Distance(transform.position, player.position) <= sightRange)
			playerInSightRange = true;
		else
			playerInSightRange = false;

		if (Vector3.Distance(transform.position, player.position) <= attackRange)
			playerInAttackRange = true;
		else
			playerInAttackRange = false;

		//float distance = Vector3.Distance(transform.position, agent);
		//float finalSpeed = (distance / speed);
		//transform.position = Vector3.Lerp(transform.position, agent, Time.deltaTime / finalSpeed);
		transform.position =  Vector3.MoveTowards(transform.position, agent, speed);


		if (!playerInSightRange && !playerInAttackRange) Patroling();
		if (playerInSightRange && !playerInAttackRange) ChasePlayer();
		if (playerInAttackRange && playerInSightRange) AttackPlayer();
	}

	private void Patroling() {
		if (!walkPointSet) SearchWalkPoint();

		if (walkPointSet)
			agent = walkPoint;

		Vector3 distanceToWalkPoint = transform.position - walkPoint;

		//Walkpoint reached
		if (distanceToWalkPoint.magnitude < 1f)
			walkPointSet = false;

		transform.LookAt(agent);
	}
	private void SearchWalkPoint() {
		//Calculate random point in range
		float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
		float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

		walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

		//if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
			walkPointSet = true;
	}

	private void ChasePlayer() {
		agent = player.position;

		Vector3 targetDirection = agent - transform.position;
		targetDirection.y = 0;
		float singleStep = 5 * Time.deltaTime;
		Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
		Debug.DrawRay(transform.position, newDirection, Color.red);
		transform.rotation = Quaternion.LookRotation(newDirection);
	}

	private void AttackPlayer() {
		//Make sure enemy doesn't move
		agent = transform.position;



		Vector3 targetDirection = player.position - transform.position;
		targetDirection.y = 0;
		float singleStep = 5 * Time.deltaTime;
		Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
		Debug.DrawRay(transform.position, newDirection, Color.red);
		transform.rotation = Quaternion.LookRotation(newDirection);



		if (weapon.IsCanAttack())
			weapon.DoSingleAttack();

			
	}


	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, sightRange);
	}
}