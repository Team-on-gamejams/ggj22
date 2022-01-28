using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.Weapons;
using BattleSystem.Health;

public class emeny_AI_healer : MonoBehaviour {
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
		start_pos = transform.position;
		rb = this.GetComponent<Rigidbody>();


		int hp = 100;

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			if (go != gameObject &&
				go.TryGetComponent(out emeny_AI em) &&
				go.GetComponent<Health>().CurrHealth <= hp
				&& Vector3.Distance(transform.position, go.transform.position) < sightRange) {

				player = go.transform;
				hp = go.GetComponent<Health>().CurrHealth;
			}
		}

		if (player == null) {
			gameObject.GetComponent<emeny_AI>().enabled = true;
			gameObject.GetComponent<emeny_AI_healer>().enabled = false;
		}

	}

	private void Update() {

		int hp = 100;

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			if (go != gameObject &&
			go.TryGetComponent(out emeny_AI em) &&
			go.GetComponent<Health>().CurrHealth <= hp
			&& Vector3.Distance(transform.position, go.transform.position) < sightRange) {

				player = go.transform;
				hp = go.GetComponent<Health>().CurrHealth;
			}
		}

		if (player == null) {
			gameObject.GetComponent<emeny_AI>().enabled = true;
			gameObject.GetComponent<emeny_AI_healer>().enabled = false;
		}


		if (Vector3.Distance(transform.position, player.position) <= sightRange)
			playerInSightRange = true;
		else
			playerInSightRange = false;

		if (Vector3.Distance(transform.position, player.position) <= attackRange)
			playerInAttackRange = true;
		else
			playerInAttackRange = false;


		Vector3 dir = agent - transform.position;
		dir.y = transform.position.y;
		rb.velocity = dir * speed;

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


		if (weapon.IsCanAttack() 
			&& player.gameObject.GetComponent<Health>().CurrHealth <
			player.gameObject.GetComponent<Health>().MaxHealth) {

			weapon.DoSingleAttack();
		}
	}


}