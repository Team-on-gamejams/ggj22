using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BattleSystem.Weapons.UI {
	public class WeaponIcon : MonoBehaviour {
		[Header("Refs"), Space]
		[SerializeField] BaseWeapon weapon;

		[Header("Refs - self"), Space]
		[SerializeField] Image weaponImage;
		[SerializeField] Image cooldownImage;
		[SerializeField] public TextMeshProUGUI cooldownTextField;

		private void Awake() {
			weaponImage.sprite = weapon.UIData.image;
		}

		void OnEnable() {
			OnCooldownUpdate(1, 1);
			weapon.onCooldownUpdate += OnCooldownUpdate;
		}

		void OnDisable() {
			weapon.onCooldownUpdate -= OnCooldownUpdate;
		}

		void OnCooldownUpdate(float curr, float max) {
			float left = Mathf.Clamp(max - curr, 0, max);

			cooldownTextField.text = left.ToString("0.00");
			cooldownImage.fillAmount = 1.0f - Mathf.Clamp01(curr / max);

			cooldownTextField.gameObject.SetActive(curr < max);
		}
	}
}
