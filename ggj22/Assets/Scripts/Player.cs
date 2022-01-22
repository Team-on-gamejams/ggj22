using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem.Weapons;
using Invector.vCharacterController;

public class Player : MonoBehaviour {
	[Header("Refs - per class"), Space]
	[SerializeField] BaseWeapon[] attacks;
	bool[] attacksDown;

	[Header("Refs - general"), Space]
	[SerializeField] PlayerInputHandler inputs;
	[SerializeField] vThirdPersonController thirdPersonController;



#if UNITY_EDITOR
	private void Reset() {
		inputs = GetComponent<PlayerInputHandler>();
	}
#endif

	private void Awake() {
		attacksDown = new bool[attacks.Length];
	}

	private void OnEnable() {
		SubscribeInputs();
	}

	private void OnDisable() {
		UnSubscribeInputs();
	}

	private void Update() {
		StringBuilder sb = new StringBuilder(32);

		for(int i = 0; i < attacks.Length; ++i) {
			sb.Append($"{attacksDown[i]}-{attacks[i].IsAttacking()}");
			if (i != attacks.Length - 1)
				sb.Append(" | ");
		}

		Debug.Log(sb.ToString());
	}

	#region Inputs
	void SubscribeInputs() {
		inputs.attackButtonDown += AttackDown;
		inputs.attackButtonUp += AttackUp;

		inputs.useItem += UseItem;
		inputs.useSpell += UseSpell;

		inputs.map += MapToggle;

		inputs.dodge += Dodge;
		inputs.toggleRun += ToggleRun;
		inputs.interact += Interact;
	}

	void UnSubscribeInputs() {
		inputs.attackButtonDown -= AttackDown;
		inputs.attackButtonUp -= AttackUp;

		inputs.useItem -= UseItem;
		inputs.useSpell -= UseSpell;

		inputs.map -= MapToggle;

		inputs.dodge -= Dodge;
		inputs.toggleRun -= ToggleRun;
		inputs.interact -= Interact;
	}

	void AttackDown(int id) {
		attacksDown[id] = true;

		ProcessAttacks(id);
	}

	void AttackUp(int id) {
		attacksDown[id] = false;

		ProcessAttacks(id);
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
		thirdPersonController.Sprint(!thirdPersonController.isSprinting);
	}

	void Interact() {
		Debug.Log($"Interact");
	}

	void ProcessAttacks(int id) {
		if (attacksDown[id]) {
			thirdPersonController.Strafe(true);
			thirdPersonController.Sprint(false);

			for (int i = 0; i < attacks.Length; ++i) {
				if (i != id && attacks[i].IsAttacking()) {
					if (attacks[i].IsCanInterruptAttack()) {
						attacks[i].InterruptAttack();
						attacks[i].OnInputActionUp();
						attacks[id].OnInputActionDown();
						return;
					}
					else{
						attacks[i].onEndAttack += DoAttackThis;

						return;
					}
				}

				void DoAttackThis() {
					attacks[i].onEndAttack -= DoAttackThis;
					attacks[i].OnInputActionUp();
					attacks[id].OnInputActionDown();
				}
			}

			attacks[id].OnInputActionDown();
		}
		else {
			bool isAnyAttackDown = false;

			for(int i = 0; i < attacksDown.Length; ++i) {
				if (attacksDown[i]) {
					isAnyAttackDown = true;
					break;
				}
			}

			if (isAnyAttackDown) {
				if (attacks[id].IsCanInterruptAttack()) {
					attacks[id].InterruptAttack();
					attacks[id].OnInputActionUp();

					for (int i = 0; i < attacks.Length; ++i) {
						if (attacksDown[i]) {
							attacks[i].OnInputActionDown();
							break;
						}
					}
				}
				else {
					attacks[id].onEndAttack += DoAttackOther;
				}
			}
			else {
				attacks[id].OnInputActionUp();
				thirdPersonController.Strafe(false);
			}
		}

		void DoAttackOther() {
			attacks[id].onEndAttack -= DoAttackOther;
			attacks[id].OnInputActionUp();

			for (int i = 0; i < attacks.Length; ++i) {
				if (attacksDown[i]) {
					attacks[i].OnInputActionDown();
					break;
				}
			}
		}
	}
	#endregion
}
