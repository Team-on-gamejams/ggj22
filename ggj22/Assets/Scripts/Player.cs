using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] PlayerInputHandler inputs;

#if UNITY_EDITOR
	private void Reset() {
		inputs = GetComponent<PlayerInputHandler>();
	}
#endif

	private void OnEnable() {
		inputs.attackButtonDown += AttackDown;
		inputs.attackButtonUp += AttackUp;

		inputs.useItem += UseItem;
		inputs.useSpell += UseSpell;

		inputs.map += MapToggle;

		inputs.dodge += Dodge;
		inputs.toggleRun += ToggleRun;
		inputs.interact += Interact;
	}

	private void OnDisable() {
		inputs.attackButtonDown -= AttackDown;
		inputs.attackButtonUp -= AttackUp;

		inputs.useItem -= UseItem;
		inputs.useSpell -= UseSpell;

		inputs.map -= MapToggle;

		inputs.dodge -= Dodge;
		inputs.toggleRun -= ToggleRun;
		inputs.interact -= Interact;
	}

	#region Inputs
	void AttackDown(int id) {
		Debug.Log($"attack down {id}");

	}

	void AttackUp(int id) {
		Debug.Log($"attack up {id}");

	}

	void UseSpell(int id) {
		Debug.Log($"Use spell {id}");
	}

	void UseItem(int id) {
		Debug.Log($"Use item {id}");
	}

	void MapToggle(bool state) {
		Debug.Log($"map toggle {state}");
	}

	void Dodge() {
		Debug.Log($"Dodge");
	}

	void ToggleRun () {
		Debug.Log($"ToggleRun");
	}

	void Interact() {
		Debug.Log($"Interact");
	}
	#endregion
}
