using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Health.Feedback {
	[RequireComponent(typeof(Health))]
	public class HealthAudio : MonoBehaviour {
		[Header("Audio"), Space]
		[SerializeField] AudioClip[] onGetHealClip;
		[SerializeField] AudioClip[] onGetDamageClip;
		[SerializeField] AudioClip[] onDieClip;

		[Header("Refs"), Space]
		[SerializeField] Health health;

#if UNITY_EDITOR
		private void Reset() {
			health = GetComponent<Health>();
		}
#endif

		private void OnEnable() {
			if ((onGetHealClip != null && onGetHealClip.Length != 0) || (onGetDamageClip != null && onGetDamageClip.Length != 0))
				health.onGetDamage += OnGetDamage;
			if (onDieClip != null && onDieClip.Length != 0)
				health.onDie += OnDie;
		}

		private void OnDisable() {
			if ((onGetHealClip != null && onGetHealClip.Length != 0) || (onGetDamageClip != null && onGetDamageClip.Length != 0))
				health.onGetDamage -= OnGetDamage;
			if (onDieClip != null && onDieClip.Length != 0)
				health.onDie -= OnDie;
		}

		void OnGetDamage(Health.HealthCallbackData data) {
			if (data.isDie)
				return;

			if (data.recievedDamage > 0) {
				if (onGetHealClip != null && onGetHealClip.Length != 0)
					AudioManager.Instance.Play3D(onGetHealClip.Random(), transform);
			}
			else {
				if (onGetDamageClip != null && onGetDamageClip.Length != 0)
					AudioManager.Instance.Play3D(onGetDamageClip.Random(), transform);
			}
		}

		void OnDie() {
			AudioManager.Instance.Play3D(onDieClip.Random(), transform.position);
		}
	}
}
