using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons.Range.Projectiles {
	public abstract class BaseProjectile : MonoBehaviour {
		protected Damage damage;
		protected ProjectileWeapon.ProjectileValues projectileValues;

		protected Action onHit;
		protected Action onMiss;

		public virtual void Init(Damage _damage, ProjectileWeapon.ProjectileValues _projectileValues, Action _onHit, Action _onMiss) {
			damage = _damage;
			projectileValues = _projectileValues;
			onHit = _onHit;
			onMiss = _onMiss;
		}

		public abstract void Launch();
	}
}
