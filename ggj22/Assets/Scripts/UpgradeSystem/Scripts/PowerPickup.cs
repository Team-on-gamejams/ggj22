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
		[SerializeField] BuffIcon buffIcon;
		[SerializeField] BuffIcon debuffIcon;
		[SerializeField] ConditionIcon condionIcon;

		[Header("Global data"), Space]
		[SerializeField] SerializedDictionary<PowerCondition, Sprite> conditionSprites;
		[SerializeField] SerializedDictionary<PowerPair, Sprite> pairsSprites;

		Power power;

		private void Awake() {
			power = new Power() {
				condition = (PowerCondition)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PowerCondition)).Length),
				pair = (PowerPair)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PowerPair)).Length),
				buffPower = 1.25f,
				debuffPower = 0.85f,
			};

			buffIcon.SetSprite(power.pair, pairsSprites, true);
			buffIcon.SetSprite(power.pair, pairsSprites, false);

			condionIcon.SetSprite(power.condition, conditionSprites);
		}

		public void Pickup() {
			PowersManager.instance.AddPower(power);
			Destroy(gameObject);
		}
	}
}
