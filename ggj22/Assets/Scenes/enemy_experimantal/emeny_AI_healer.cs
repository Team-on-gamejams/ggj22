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

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
			if (go != gameObject &&
				go.TryGetComponent(out emeny_AI em) &&
				em.playerInSightRange) {

				player = go.transform;
				break;
			}
		}

		if (player == null) {
			gameObject.GetComponent<emeny_AI>().enabled = true;
			gameObject.GetComponent<emeny_AI_healer>().enabled = false;
		}

	}

	private void Update() {
		//Check for sight and attack range
		//playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
		//playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
		if (player != null) {
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
			transform.position = Vector3.MoveTowards(transform.position, agent, speed);


			if (!playerInSightRange && !playerInAttackRange) Patroling();
			if (playerInSightRange && !playerInAttackRange) ChasePlayer();
			if (playerInAttackRange && playerInSightRange) AttackPlayer();
		}
		else {
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
				if (go != gameObject && 
					go.TryGetComponent(out emeny_AI em) && 
					em.playerInSightRange) {

					player = go.transform;
					break;
				}
			}

			if (player == null) {
				gameObject.GetComponent<emeny_AI>().enabled = true;
				gameObject.GetComponent<emeny_AI_healer>().enabled = false;
			}
		}
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
		if (Vector3.Distance(transform.position, player.transform.position) >= attackRange - 1.5)
			agent = player.position;
		else
			agent = transform.position;

		

		Vector3 targetDirection = player.position - transform.position;
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




		if (weapon.IsCanAttack() 
			&& player.gameObject.GetComponent<Health>().CurrHealth <
			player.gameObject.GetComponent<Health>().MaxHealth) {

			weapon.DoSingleAttack();
			//Debug.Log("heal");
		}


		///End of attack code



	}


}