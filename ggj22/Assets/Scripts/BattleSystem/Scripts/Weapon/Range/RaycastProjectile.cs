using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem.Health;

namespace BattleSystem.Weapons.Range.Projectiles {
	public class RaycastProjectile : BaseProjectile {
		byte len;
		RaycastHit[] hits;
		float movePerFrame;

		byte i, j;
		Health.Health health;
		HealthHitbox hitboxMin;
		HealthHitbox hitbox;
		bool isHit;
		float distMin;
		float dist;

		float flyedDist;

		private void Awake() {
			hits = new RaycastHit[4];
		}

		private void Update() {
			movePerFrame = projectileValues.speed * Time.deltaTime;
			flyedDist += movePerFrame;


			isHit = false;
			RaycastAndCheckHit(LayerMask.GetMask("Hitbox"), QueryTriggerInteraction.Collide);
			if (isHit) {
				onHit?.Invoke();
			}

			if (!isHit)
				RaycastIsHitAnything(LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);

			if (isHit) {
				Stop();
				onMiss?.Invoke();
			}
			else {
				transform.position += transform.forward * movePerFrame;
				if (flyedDist > projectileValues.maxFlyLength) {
					Stop();
				}
			}
		}

		public override void Init(Damage _damage, ProjectileWeapon.ProjectileValues projectileValues, Action _onHit, Action _onMiss) {
			base.Init(_damage, projectileValues, _onHit, _onMiss);
			flyedDist = 0;
			enabled = false;
		}

		public override void Launch() {
			enabled = true;
		}

		void Stop() {
			Destroy(gameObject);
		}

		void RaycastAndCheckHit(int layerMask, QueryTriggerInteraction queryTriggerInteraction) {
			len = (byte)Physics.RaycastNonAlloc(
				transform.position,
				transform.forward,
				hits,
				movePerFrame * 1.5f,
				layerMask,
				queryTriggerInteraction
			);

			if (len != 0) {
				for (i = 0; i < len; ++i) {
					hitbox = hits[i].collider.GetComponent<HealthHitbox>();

					if (hitbox) {
						dist = (hitbox.transform.position - transform.position).sqrMagnitude;

						if (!hitboxMin || distMin > dist) {
							hitboxMin = hitbox;
							distMin = dist;
						}
					}
				}

				if (hitboxMin) {
					if (hitboxMin.GetDamage(damage) != 0) {
						isHit = true;
					}
				}
			}
		}

		void RaycastIsHitAnything(int layerMask, QueryTriggerInteraction queryTriggerInteraction) {
			len = (byte)Physics.RaycastNonAlloc(
				transform.position,
				transform.forward,
				hits,
				movePerFrame,
				layerMask,
				queryTriggerInteraction
			);


			if (len != 0) {
				for (i = 0; i < len && !isHit; ++i) {
					if ((health = hits[i].collider.GetComponent<Health.Health>())) {
						if (health.Armor.fraction != damage.fraction) {
							isHit = true;
						}
					}
					else if (hitbox = hits[i].collider.GetComponent<HealthHitbox>()) {
						if (!hitbox.IsSameFraction(damage.fraction)) {
							isHit = true;
						}
					}
					else {
						isHit = true;
					}
				}
			}
		}
	}
}
