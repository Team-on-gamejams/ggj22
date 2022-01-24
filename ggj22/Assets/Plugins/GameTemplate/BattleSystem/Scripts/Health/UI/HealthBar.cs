using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BattleSystem.Health.UI {
	public class HealthBar : MonoBehaviour {
		[Header("UI")]
		[SerializeField] Slider firstSlider;
		[SerializeField] Slider secondSlider;
		[SerializeField] Image background;
		[SerializeField] TextMeshProUGUI textField;

		[Header("Refs"), Space]
		[SerializeField] Health health;

#if UNITY_EDITOR
		private void Reset() {
			health = GetComponent<Health>();
		}
#endif

		private void OnEnable() {
			health.onReInit += OnReinit;
			health.onGetDamage += OnGetDamage;
		}

		private void OnDisable() {
			health.onReInit -= OnReinit;
			health.onGetDamage -= OnGetDamage;
		}

		void OnReinit() {
			ReInit();
		}

		void OnGetDamage(Health.HealthCallbackData data) {
			UpdateVisuals(false);
		}

		void ReInit() {
			firstSlider.minValue = secondSlider.minValue = 0;
			firstSlider.maxValue = secondSlider.maxValue = health.MaxHealth;

			UpdateVisuals(true);
		}

		void UpdateVisuals(bool isForce) {
			if (textField)
				textField.text = $"{health.CurrHealth} / {health.MaxHealth}";

			LeanTween.cancel(gameObject);

			if (isForce) {
				firstSlider.value = secondSlider.value = health.CurrHealth;
			}
			else {
				LeanTween.value(firstSlider.value, health.CurrHealth, 0.2f)
				.setEase(LeanTweenType.linear)
				.setOnUpdate((float val) => {
					firstSlider.value = val;
				});

				LeanTween.value(secondSlider.value, health.CurrHealth, 0.7f)
				.setEase(LeanTweenType.easeInOutQuart)
				.setOnUpdate((float val) => {
					secondSlider.value = val;
				});
			}
		}
	}
}
