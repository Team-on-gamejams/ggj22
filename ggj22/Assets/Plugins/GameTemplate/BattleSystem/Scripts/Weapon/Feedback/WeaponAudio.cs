using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons.Feedback
{
	[RequireComponent(typeof(BaseWeapon))]
	public class WeaponAudio : MonoBehaviour
    {
		[Header("Audio"), Space]
		[SerializeField] AudioClip[] onAttackStart;
		[SerializeField] AudioClip[] onAttackHit;
		[SerializeField] AudioClip[] onAttackMiss;

		[Header("Refs"), Space]
		[SerializeField] BaseWeapon weapon;

#if UNITY_EDITOR
		private void Reset() {
			weapon = GetComponent<BaseWeapon>();
		}
#endif

		private void OnEnable() {
			if (onAttackStart != null && onAttackStart.Length != 0)
				weapon.onStartAttack += OnStartAttack;
			if (onAttackHit != null && onAttackHit.Length != 0)
				weapon.onHitAttack += OnHitAttack;
			if (onAttackMiss != null && onAttackMiss.Length != 0)
				weapon.onMissAttack += OnMissAttack;
		}

		private void OnDisable() {
			if (onAttackStart != null && onAttackStart.Length != 0)
				weapon.onStartAttack -= OnStartAttack;
			if (onAttackHit != null && onAttackHit.Length != 0)
				weapon.onHitAttack -= OnHitAttack;
			if (onAttackMiss != null && onAttackMiss.Length != 0)
				weapon.onMissAttack -= OnMissAttack;
		}

		void OnStartAttack() {
			AudioManager.Instance.Play3D(onAttackStart.Random(), transform.position);
		}
		void OnHitAttack() {
			AudioManager.Instance.Play3D(onAttackHit.Random(), transform.position);
		}
		void OnMissAttack() {
			AudioManager.Instance.Play3D(onAttackMiss.Random(), transform.position);
		}
	}
}
