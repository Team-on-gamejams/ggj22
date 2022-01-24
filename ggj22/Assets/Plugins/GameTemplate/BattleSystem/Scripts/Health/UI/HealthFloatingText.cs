using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Health.UI {
	[RequireComponent(typeof(Health))]
	public class HealthFloatingText : MonoBehaviour {
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
			int offsets = 0;

			if (data.isLastChance) ++offsets;
			if (data.isDie) ++offsets;
			if (data.armorType == ArmorType.ArmoredArmor) ++offsets;


			FloatingText text = Instantiate(floatingTextPrefabForNumbers, textAnchor.position + Vector3.up * offsets * 0.3f, Quaternion.identity).GetComponent<FloatingText>();
			if (data.recievedDamage > 0) {
				text.Play(data.recievedDamage.ToString(), data.damageType.ToColor(data.armorType));
			}
			else {
				text.Play(Mathf.Abs(data.recievedDamage).ToString(), data.damageType.ToColor(data.armorType));
			}
			--offsets;

			if (data.isLastChance) {
				text = Instantiate(floatingTextPrefabForText, textAnchor.position, Quaternion.identity).GetComponent<FloatingText>();
				text.PlayLocalized("BATTLE_SYSTEM_LAST_CHANCE", Color.red);
				--offsets;
			}

			if (data.armorType == ArmorType.ArmoredArmor) {
				text = Instantiate(floatingTextPrefabForText, textAnchor.position, Quaternion.identity).GetComponent<FloatingText>();
				text.PlayLocalized("BATTLE_SYSTEM_ARMORED", Color.gray);
				--offsets;
			}
		}

		void OnDie() {
			FloatingText text = Instantiate(floatingTextPrefabForText, textAnchor.position, Quaternion.identity).GetComponent<FloatingText>();
			text.PlayLocalized("BATTLE_SYSTEM_DIE", Color.red);
		}
	}
}
