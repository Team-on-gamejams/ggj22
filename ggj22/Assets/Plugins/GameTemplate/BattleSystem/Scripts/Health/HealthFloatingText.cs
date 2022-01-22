using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
	[RequireComponent(typeof(Health))]
    public class HealthFloatingText : MonoBehaviour
    {
		[Header("Refs"), Space]
		[SerializeField] Health health;
		[SerializeField] Transform textAnchor;

		[Header("Refs - prefabs"), Space]
		[SerializeField] GameObject floatingTextPrefabForNumbers;
		[SerializeField] GameObject floatingTextPrefabForText;

#if UNITY_EDITOR
		private void Reset() {
			health = GetComponent<Health>();
		}
#endif

		private void OnEnable() {
			health.onGetDamage += OnGetDamage;
			health.onDie += OnDie;
		}

		private void OnDisable() {
			health.onGetDamage -= OnGetDamage;
			health.onDie -= OnDie;
		}

		void OnGetDamage(Health.HealthCallbackData data) {
			FloatingText text = Instantiate(floatingTextPrefabForNumbers, textAnchor.position + Vector3.up * (data.isLastChance || data.isDie ? 0.5f : 0f), Quaternion.identity).GetComponent<FloatingText>();

			if (data.recievedDamage > 0) {
				text.Play(data.recievedDamage.ToString(), data.type.ToColor());
			}
			else {
				text.Play(Mathf.Abs(data.recievedDamage).ToString(), data.type.ToColor());
			}

			if (data.isLastChance) {
				text = Instantiate(floatingTextPrefabForText, textAnchor.position, Quaternion.identity).GetComponent<FloatingText>();
				text.PlayLocalized("BATTLE_SYSTEM_LAST_CHANCE", Color.red);
			}
		}

		void OnDie() {
			FloatingText text = Instantiate(floatingTextPrefabForText, textAnchor.position, Quaternion.identity).GetComponent<FloatingText>();
			text.PlayLocalized("BATTLE_SYSTEM_DIE", Color.red);
		}
	}
}
