using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.Weapons;
using BattleSystem.Health;

public class emeny_AI_range : MonoBehaviour {
	[SerializeField] BaseWeapon weapon;
	public Health health;

	public event Action EnemyAttack;
	public event Action Patrooling;
	public event Action Chasing;

	public Vector3 agent;

	private Vector3 start_pos;

	public Rigidbody rb;

	public Vector2 movement;

	public Transform player; //enemies

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
		rb = this.GetComponent<Rigidbody>();
	}

	private void Start() {
		start_pos = transform.position;
	}

	private void Update() {
		if (Vector3.Distance(transform.position, player.position) <= sightRange)
			playerInSightRange = true;
		else
			playerInSightRange = false;

		if (Vector3.Distance(transform.position, player.position) <= attackRange)
			playerInAttackRange = true;
		else
			playerInAttackRange = false;


		if (!playerInSightRange && !playerInAttackRange) Patroling();
		if (playerInSightRange && !playerInAttackRange) ChasePlayer();
		if (playerInAttackRange && playerInSightRange) AttackPlayer();

		Vector3 dir = agent - transform.position;
		dir.y = transform.position.y;
		rb.velocity = dir * speed;

	}

	private void Patroling() {
		if (!walkPointSet) SearchWalkPoint();

		if (walkPointSet)
			agent = walkPoint;

		Vector3 distanceToWalkPoint = transform.position - walkPoint;

		//Walkpoint reached
		if (distanceToWalkPoint.magnitude < 1f)
			walkPointSet = false;

		transform.LookAt(new Vector3(agent.x, 0, agent.z));
	}

	private void SearchWalkPoint() {
		float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
		float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

		walkPoint = new Vector3(start_pos.x + + randomX, transform.position.y, start_pos.z + randomZ);

		if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
			walkPointSet = true;
	}

	private void ChasePlayer() {
		agent = player.position;
		transform.LookAt(new Vector3(player.position.x, 0, player.position.z));

		if(Vector3.Distance(player.position, transform.position) <= attackRange + 4)
			rb.drag = 40;
	}

	private void AttackPlayer() {
		
		agent = transform.position;
		transform.LookAt(new Vector3(player.position.x, 0, player.position.z));


		if (weapon.IsCanAttack()) {

			weapon.DoSingleAttack();
		}
	}


}