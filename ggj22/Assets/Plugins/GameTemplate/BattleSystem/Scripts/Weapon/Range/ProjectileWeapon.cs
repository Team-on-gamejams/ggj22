using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons.Range { 
	public class ProjectileWeapon : BaseWeapon {
		protected override void DoAttack() {
			Debug.Log("Projectile attack");
		}
	}
}
