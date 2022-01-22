using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons {
	public class WeaponAnimationCallbacks : MonoBehaviour {
		[Header("Refs"), Space]
		[SerializeField] BaseWeapon[] weapons;

		public void PerformAttack() {
			for(int i = 0; i < weapons.Length; ++i)
				weapons[i].AnimationCallbackPerformAttack();
		}

		public void EndAttack() {
			for(int i = 0; i < weapons.Length; ++i)
				weapons[i].AnimationCallbackEndAttack();
		}
	}
}
