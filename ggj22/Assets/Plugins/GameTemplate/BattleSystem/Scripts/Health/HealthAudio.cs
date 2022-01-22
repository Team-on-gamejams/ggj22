using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
	[RequireComponent(typeof(Health))]
	public class HealthAudio : MonoBehaviour
    {
		[Header("Audio"), Space]
		[SerializeField] AudioClip onGetHealClip;
		[SerializeField] AudioClip onGetDamageClip;
		[SerializeField] AudioClip onDieClip;

		[Header("Refs"), Space]
		[SerializeField] Health health;

#if UNITY_EDITOR
		private void Reset() {
			health = GetComponent<Health>();
		}
#endif

		private void OnEnable() {
			if(onGetHealClip || onGetDamageClip)
				health.onGetDamage += OnGetDamage;
			if(onDieClip)
				health.onDie += OnDie;
		}

		private void OnDisable() {
			if(onGetHealClip || onGetDamageClip)
				health.onGetDamage -= OnGetDamage;
			if(onDieClip)
				health.onDie -= OnDie;
		}

		void OnGetDamage(Health.HealthCallbackData data) {
			if (data.isDie)
				return;

			if(data.recievedDamage > 0) {
				if (onGetHealClip)
					AudioManager.Instance.Play3D(onGetHealClip, transform);
			}
			else {
				if (onGetDamageClip)
					AudioManager.Instance.Play3D(onGetDamageClip, transform);
			}
		}

		void OnDie() {
			AudioManager.Instance.Play3D(onDieClip, transform.position);
		}
	}
}
