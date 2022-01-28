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
	public bool CanMove = true;

	public event Action EnemyAttack;
	public event Action Patrooling;
	public event Action Chasing;

	public Vector3 agent;

	private Vector3 start_pos;

	public Rigidbody rb;

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
		start_pos = transform.position;
		rb = this.GetComponent<Rigidbody>();

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



		if (CanMove) {
			//transform.position =  Vector3.MoveTowards(transform.position, agent, speed);

			Vector3 dir = agent - transform.position;
			dir.y = transform.position.y;
			rb.velocity = dir * speed;

			transform.LookAt(new Vector3(agent.x, 0, agent.z));

		}
			


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

	}
	private void SearchWalkPoint() {
		//Calculate random point in range
		float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
		float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

		walkPoint = new Vector3(start_pos.x + randomX, transform.position.y, start_pos.z + randomZ);

		if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
			walkPointSet = true;
	}

	private void ChasePlayer() {

		agent = player.position;
	}

	private void AttackPlayer() {

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