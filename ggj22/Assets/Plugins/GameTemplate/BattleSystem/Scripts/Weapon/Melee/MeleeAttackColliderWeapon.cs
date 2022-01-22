using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem.Health;

namespace BattleSystem.Weapons.Melee {
	public class MeleeAttackColliderWeapon : BaseWeapon {
		[Header("Refs - melee"), Space]
		[SerializeField] Collider attackCollider;

		Collider[] colliders = null;
		List<HealthHitbox> hitboxes = new List<HealthHitbox>(1);
		HealthHitbox hitbox;

		byte i, j;
		bool findSame;
		float dist1;
		float dist2;

		void Start() {
			attackCollider.gameObject.SetActive(false);
		}

		protected override void DoAttack() {
			colliders = null;
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
					Debug.LogError($"Not implemented collider type for {gameObject.name}");
					break;

				case SphereCollider sphere:
					colliders = Physics.OverlapSphere(
						attackCollider.transform.position + sphere.center,
						sphere.radius * attackCollider.transform.lossyScale.x,
						LayerMask.GetMask("Hitbox"),
						QueryTriggerInteraction.Collide
					);
					break;

				default:
					Debug.LogError($"Wrong collider type for {gameObject.name}");
					return;
			}

			if(colliders.Length == 0) {
				return;
			}
			else {
				hitboxes.Clear();
			}

			for (i = 0; i < colliders.Length; ++i) {
				hitbox = colliders[i].GetComponent<HealthHitbox>();

				if (hitbox) {
					findSame = false;
					dist1 = -1;

					for (j = 0; j < hitboxes.Count; ++j) {
						if (hitbox.IsSameParent(hitboxes[j])) {
							findSame = true;
							if (dist1 == -1)
								dist1 = (hitbox.transform.position - attackCollider.transform.position).sqrMagnitude;
							dist2 = (hitboxes[j].transform.position - attackCollider.transform.position).sqrMagnitude;


							if (dist1 < dist2) {
								hitboxes.Remove(hitboxes[j]);
								hitboxes.Add(hitbox);
							}
							break;
						}
					}

					if (!findSame)
						hitboxes.Add(hitbox);
				}
			}

			for (i = 0; i < hitboxes.Count; ++i) {
				hitboxes[i].GetDamage(damage);
			}
		}
	}
}
