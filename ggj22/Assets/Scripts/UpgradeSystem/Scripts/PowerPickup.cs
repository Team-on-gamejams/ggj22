using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityForge;
using UnityForge.PropertyDrawers;
using UpgradeSystem.UI;

namespace UpgradeSystem {
	public class PowerPickup : MonoBehaviour {
		public Power Power => power;

		[Header("Popup"), Space]
		[SerializeField] PowerPopup popup;

		[Header("Power"), Space]
		[SerializeField] bool isRandomPower = true;
		[SerializeField] Power power;

		private void Start() {
			if (isRandomPower) {
				power = Power.GetRandomPower();
			}

			popup.Init(power, true);
		}

		public void Pickup() {
			PowersManager.Instance.AddPower(power);
			Destroy(gameObject);
		}
	}
}
