using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons.Range.Projectiles {
	public abstract class BaseProjectile : MonoBehaviour {
		protected Damage damage;
		protected ProjectileWeapon.ProjectileValues projectileValues;

		public virtual void Init(Damage _damage, ProjectileWeapon.ProjectileValues _projectileValues) {
			damage = _damage;
			projectileValues = _projectileValues;
		}

		public abstract void Launch();
	}
}
