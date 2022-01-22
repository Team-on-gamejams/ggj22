using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons.Utils {
	[RequireComponent(typeof(BaseWeapon))]
    public class WeaponAutoAttack : MonoBehaviour
    {

		[Header("Refs"), Space]
		[SerializeField] BaseWeapon weapon;

#if UNITY_EDITOR
		private void Reset() {
			weapon = GetComponent<BaseWeapon>();
		}
#endif

		private void Update() {
			if(weapon.IsCanAttack())
				weapon.DoSingleAttack();
		}
	}
}
