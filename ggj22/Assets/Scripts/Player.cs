using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem.Health;
using BattleSystem.Weapons;
using Invector.vCharacterController;
using PickupSystem;
using UpgradeSystem;

public class Player : MonoBehaviour {
	[Header("Refs - per class"), Space]
	[SerializeField] BaseWeapon[] attacks;
	bool[] attacksDown;

	[Header("Refs - Map"), Space]
	[SerializeField] RenderTexture mapTexture;
	[SerializeField] RawImage minimapImage;
	[SerializeField] RawImage mapImage;

	[Header("Refs - general"), Space]
	[SerializeField] PlayerInputHandler inputs;
	[SerializeField] vThirdPersonController thirdPersonController;
	[SerializeField] vThirdPersonCamera camera;
	[SerializeField] Camera minimapCamera;
	[SerializeField] Health health;

#if UNITY_EDITOR
	private void Reset() {
		inputs = GetComponent<PlayerInputHandler>();
	}
#endif

	private void Awake() {
		attacksDown = new bool[attacks.Length];
		Cursor.lockState = CursorLockMode.Locked;
		ToggleMapToMinimap();
	}

	private void OnDestroy() {
		Cursor.lockState = CursorLockMode.None;

		SceneLoader.Instance.LoadScene(1, true, true);
	}

	private void OnEnable() {
		SubscribeInputs();

		PowersManager.Instance.onPowersReapply += ReapplyPowers;
	}

	private void OnDisable() {
		UnSubscribeInputs();

		PowersManager.Instance.onPowersReapply -= ReapplyPowers;
	}

	private void Update() {
		PowersManager.Instance.IsMoving = thirdPersonController.input != Vector3.zero;
	}

	void ReapplyPowers(Dictionary<PowerPair, float> powers) {
		foreach (var power in powers) {
			switch (power.Key) {
				case PowerPair.TimeControl:
					Time.timeScale = Mathf.Clamp(power.Value, 0.75f, 1.25f);
					break;

				case PowerPair.AttackSpeed:
					foreach (var weapon in attacks)
						weapon?.ApplyAttackSpeedMod(power.Value);
					break;

				case PowerPair.MoveSpeed:
					thirdPersonController.SpeedModifier = power.Value;
					break;

				case PowerPair.Armor:
					health.ApplyArmorMod(power.Value);
					break;

				case PowerPair.Damage:
					foreach (var weapon in attacks) 
						weapon?.ApplyDamageMod(power.Value);
					break;

				default:
						Debug.LogError("Not inplemented power pair");
					break;
			}
		}
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
		if (state) {
			Cursor.lockState = CursorLockMode.None;
			camera.enabled = false;
			ToggleMapToBigmap();
		}
		else {
			Cursor.lockState = CursorLockMode.Locked;
			camera.enabled = true;
			ToggleMapToMinimap();
		}
	}

	void Dodge() {
		Debug.Log($"Dodge");
	}

	void ToggleRun () {
		thirdPersonController.Sprint(!thirdPersonController.isSprinting);
	}

	void Interact() {
		Pickupable.Selected?.Pickup();
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

	void ToggleMapToMinimap() {
		minimapImage.gameObject.SetActive(true);
		mapImage.gameObject.SetActive(false);

		if (minimapCamera.targetTexture != null) 
			minimapCamera.targetTexture.Release();
		minimapImage.texture = minimapCamera.targetTexture = new RenderTexture((int)minimapImage.rectTransform.rect.width, (int)minimapImage.rectTransform.rect.height, 24);
		minimapCamera.orthographicSize = 20;
	}

	void ToggleMapToBigmap() {
		minimapImage.gameObject.SetActive(false);
		mapImage.gameObject.SetActive(true);

		if (minimapCamera.targetTexture != null)
			minimapCamera.targetTexture.Release();
		mapImage.texture = minimapCamera.targetTexture = new RenderTexture((int)mapImage.rectTransform.rect.width, (int)mapImage.rectTransform.rect.height, 24);
		minimapCamera.orthographicSize = 100;
	}
	#endregion
}
