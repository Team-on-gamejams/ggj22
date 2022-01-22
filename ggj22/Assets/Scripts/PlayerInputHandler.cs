using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour {
	public event Action<int> attackButtonDown;
	public event Action<int> attackButtonUp;

	public event Action<int> useSpell;
	public event Action<int> useItem;

	public event Action toggleRun;
	public event Action dodge;
	public event Action interact;

	public event Action<bool> map;

	public void OnMap(InputAction.CallbackContext input) {
		if (input.started)
			map?.Invoke(true);
		else if (input.canceled)
			map?.Invoke(false);
	}

	public void OnInteract(InputAction.CallbackContext input) {
		if (input.performed)
			interact?.Invoke();
	}

	public void OnToggleRun(InputAction.CallbackContext input) {
		if (input.performed)
			toggleRun?.Invoke();
	}

	public void OnDodge(InputAction.CallbackContext input) {
		if (input.performed)
			dodge?.Invoke();
	}

	public void OnAttack1(InputAction.CallbackContext input) {
		if (input.started)
			attackButtonDown?.Invoke(0);
		else if (input.canceled)
			attackButtonDown?.Invoke(0);
	}

	public void OnAttack2(InputAction.CallbackContext input) {
		if (input.started)
			attackButtonDown?.Invoke(1);
		else if (input.canceled)
			attackButtonDown?.Invoke(1);
	}

	public void OnSpell1(InputAction.CallbackContext input) {
		if (input.performed)
			useSpell?.Invoke(0);
	}

	public void OnSpell2(InputAction.CallbackContext input) {
		if (input.performed)
			useSpell?.Invoke(1);
	}

	public void OnSpell3(InputAction.CallbackContext input) {
		if (input.performed)
			useSpell?.Invoke(2);
	}

	public void OnItem1(InputAction.CallbackContext input) {
		if (input.performed)
			useItem?.Invoke(0);
	}

	public void OnItem2(InputAction.CallbackContext input) {
		if (input.performed)
			useItem?.Invoke(1);
	}

	public void OnItem3(InputAction.CallbackContext input) {
		if (input.performed)
			useItem?.Invoke(2);
	}

	public void OnItem4(InputAction.CallbackContext input) {
		if (input.performed)
			useItem?.Invoke(3);
	}
}
