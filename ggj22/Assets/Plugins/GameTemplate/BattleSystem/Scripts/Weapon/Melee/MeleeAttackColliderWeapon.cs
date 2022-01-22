using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem.Health;

namespace BattleSystem.Weapons.Melee {
	public class MeleeAttackColliderWeapon : BaseWeapon {
		[Header("Refs - melee"), Space]
		[SerializeField] Collider attackCollider;

		void Start() {
			attackCollider.gameObject.SetActive(false);
		}

		protected override void DoAttack() {
			Collider[] colliders = null;
			List<HealthHitbox> hitboxes = new List<HealthHitbox>();

			switch (attackCollider) {
				case BoxCollider box:
					colliders = Physics.OverlapBox(
						attackCollider.transform.position + box.center,
						new Vector3(attackCollider.transform.lossyScale.x * box.size.x, attackCollider.transform.lossyScale.y * box.size.y, attackCollider.transform.lossyScale.z * box.size.z) / 2,
						box.transform.rotation,
						LayerMask.GetMask("Hitbox"),
						QueryTriggerInteraction.Collide
					);
					break;

				case CapsuleCollider capsule:
					break;

				case SphereCollider sphere:
					break;

				default:
					Debug.LogError($"Wrong collider type for {gameObject.name}");
					return;
			}

			foreach (var collider in colliders) {
				HealthHitbox hitbox = collider.GetComponent<HealthHitbox>();

				if (hitbox) {
					bool findSame = false;
					float dist1 = -1;
					float dist2;

					foreach (var hitbox2 in hitboxes) {
						if (hitbox.IsSameParent(hitbox2)) {
							findSame = true;
							if (dist1 == -1) {
								dist1 = (hitbox.transform.position - attackCollider.transform.position).sqrMagnitude;
							}
							dist2 = (hitbox2.transform.position - attackCollider.transform.position).sqrMagnitude;


							if (dist1 < dist2) {
								hitboxes.Remove(hitbox2);
								hitboxes.Add(hitbox);
							}
							break;
						}
					}

					if(!findSame)
						hitboxes.Add(hitbox);
				}
			}

			foreach (var hitbox in hitboxes) {
				hitbox.GetDamage(damage);
			}

		}
	}
}
