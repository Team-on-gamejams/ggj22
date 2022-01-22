using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem.Weapons.Range.Projectiles;

namespace BattleSystem.Weapons.Range { 
	public class ProjectileWeapon : BaseWeapon {
		[Header("Values"), Space]
		[SerializeField] ProjectileValues projectileValues = new ProjectileValues(20, 50);

		[Header("Refs"), Space]
		[SerializeField] Transform projectileSpawnPos;
		[SerializeField] Transform aimTo;

		[Header("Prefabs"), Space]
		[SerializeField] GameObject projectilePrefab;

		GameObject projectile;
		BaseProjectile baseProjectile;

		private void Awake() {
			if (!projectileSpawnPos)
				projectileSpawnPos = transform;
		}

		protected override void StartAttack() {
			projectile = Instantiate(projectilePrefab, projectileSpawnPos.position, projectileSpawnPos.rotation);
			baseProjectile = projectile.GetComponent<BaseProjectile>();

			baseProjectile.Init(damage, projectileValues);
		}

		protected override void DoAttack() {
			if(aimTo)
				baseProjectile.transform.forward = (aimTo.position - baseProjectile.transform.position).normalized;
			baseProjectile.Launch();

			projectile = null;
			baseProjectile = null;
		}

		[Serializable]
		public struct ProjectileValues {
			public float speed;
			public float maxFlyLength;

			public ProjectileValues(float speed, float maxFlyLength) {
				this.speed = speed;
				this.maxFlyLength = maxFlyLength;
			}
		}
	}
}
