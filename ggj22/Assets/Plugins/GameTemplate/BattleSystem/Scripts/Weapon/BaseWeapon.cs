using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons {
	public class BaseWeapon : MonoBehaviour {
		public enum WeaponState : byte { 
			Ready,			// Ready to attack
			Starting,       // Can be interrupted
			Performing,     // Single frame with actual attack
			Ending,         // Can't be interrupted
			Cooldown        // can't be used
		}

		[Header("Values"), Space]
		[SerializeField] Damage damage;

		[Header("Timings"), Space]
		[SerializeField] float cooldownTime;

		[Header("Animation"), Space]
		[SerializeField] Animator animator;
		[SerializeField] string attackAnimationName;

#if UNITY_EDITOR
		private void Reset() {
			animator = GetComponent<Animator>();
			attackAnimationName = "";

			damage = new Damage() {
				type = damage.type,
				baseDamage = damage.baseDamage, 

				isIgnoreArmor = damage.isIgnoreArmor,

				damageMods = new SerializedDictionary<ArmorType, float> {
					{ ArmorType.NormalArmor, 1.0f },
					{ ArmorType.WeakSpotArmor, 1.5f },
					{ ArmorType.ArmoredArmor, 0.5f },
				}
			};
		}
#endif

		void Update() {
			
		}

		public bool IsCanInterruptAttack() {
			return true;
		}

		public void InterruptAttack() {

		}

		#region Scripts interface
		public void DoSingleAttack() {

		}
		#endregion

		#region Player Interface
		public void OnInputActionDown() {

		}

		public void OnInputActionUp() {

		}
		#endregion
	}
}
