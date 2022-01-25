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

		[SerializeField] Power power;


		private void Awake() {
			power = new Power() {
				condition = (PowerCondition)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PowerCondition)).Length),
				pair = (PowerPair)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PowerPair)).Length),
				buffPower = 1.25f,
				debuffPower = 0.85f,
			};

			buffIcon.SetSprite(power.pair, pairsSprites, true);
			debuffIcon.SetSprite(power.pair, pairsSprites, false);

			condionIcon.SetSprite(power.condition, conditionSprites);
		}

		public void Pickup() {
			PowersManager.Instance.AddPower(power);
			Destroy(gameObject);
		}
	}
}
