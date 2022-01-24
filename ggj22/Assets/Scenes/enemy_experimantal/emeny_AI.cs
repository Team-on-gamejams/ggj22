using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class emeny_AI : MonoBehaviour
{
	[SerializeField] private bool mage = false;
	[SerializeField] private bool sworder = true;
	[SerializeField] private int intelect_level = 1; // max = 3
	[SerializeField] bool agred = false;
	[SerializeField] private GameObject player;
	private float cool_down_max = 100f; 
	private float cool_down = 0f;
	private int agro_distance = 10;
	private float attack_range = 0.5f;
	private int heal_spell = 20;
	private double f = 0;
	Vector3 point;
	
	public int hp = 100;
	private float speed = 2f;


	public int dmg = 5; //player damage



	[SerializeField] UnityEvent take_damage;
	[SerializeField] UnityEvent damage_player;


	void Start()
    {
		take_damage.AddListener(get_hurt);
		damage_player.AddListener(attack);

		player = GameObject.FindWithTag("Player");
		//find and lock player
	}
	
	void Update()
    {
		//agro if distance is <= than agro_distance
		if (Vector3.Distance(player.transform.position, transform.position) <= agro_distance) {
			agred = true;
		}

		if (agred) {
			main_AI();
		}
	}
	
	void main_AI()
    {
		if (cool_down > 0) {
			cool_down--;
		}


		if (intelect_level >= 2 && 
			cool_down <= cool_down_max - 1.5f &&
			cool_down >= 1.5f &&
			Vector3.Distance(player.transform.position, transform.position) <= attack_range) {

			defend();
		}

		if(Vector3.Distance(player.transform.position, transform.position) > attack_range) {
			approach();
		}

		if (cool_down <= 0 && 
			Vector3.Distance(player.transform.position, transform.position) <= attack_range) {

			damage_player.Invoke();
			cool_down = cool_down_max;
		}


		//if low hp

		if (hp <= 10) {


			point = transform.position;
			Vector3 vect = transform.position - player.transform.position;
			while (Vector3.Distance(point, player.transform.position) < 8) {
				point = vect + point;
			}
			


			if (intelect_level >= 2) {
				transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * speed);
			}

			if (intelect_level == 3) {
				defend();
			}



			if (Vector3.Distance(transform.position, player.transform.position) >= 5) {
				heal();
			}

		}
		//if intelligence > 1 get back and heal
		//if intelligence = 3 defend while go backwards
	}
	
	void approach()
    {
		//if intelligence = 1 go straight
		if(intelect_level == 1) {
			transform.position = 
				Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * speed);
		}
		//if intelligence >= 2 go around
		if (intelect_level >= 2) {
			//(x Ц a)2 + (y Ц b)2 = r2, где Ђaї и Ђbї -координаты центра
			
			Vector3 cent = (transform.position + player.transform.position) / 2;
			float r = Vector3.Distance(cent, transform.position);
			
				transform.position = Vector3.Lerp(transform.position,
					new Vector3(r * (float)Math.Cos(f) + cent.x,
						transform.position.y,
						r * (float)Math.Sin(f) + cent.z), //x = r cos f, y = r sin f.
					Time.deltaTime * speed);
				f+=0.05;
			
		}

	}

	void heal()
    {
		//play animation
		StartCoroutine(wait());
		hp += heal_spell;
	}

	IEnumerator wait() {
		yield return new WaitForSeconds(4);
	}

	public void attack()
    {
		//play animation
		Debug.Log("attack");
    }

	public void defend() {
		//defend with shield
	}

	public void get_hurt()
    {
		hp -= dmg;
    }

	



}
