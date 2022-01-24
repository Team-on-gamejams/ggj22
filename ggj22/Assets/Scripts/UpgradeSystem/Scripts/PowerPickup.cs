using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityForge;
using UnityForge.PropertyDrawers;

namespace UpgradeSystem
{
    public class PowerPickup : MonoBehaviour
    {
		[Header("Global data"), Space]
		[SerializeField] SerializedDictionary<PowerCondition, Sprite> conditionSprites;
		[SerializeField] SerializedDictionary<PowerPair, Sprite> pairsSprites;
		[SerializeField] Sprite buffIcon;

		Power power;

		private void Awake() {
			power = new Power() {
				condition = (PowerCondition)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PowerCondition)).Length),
				pair = (PowerPair)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PowerPair)).Length),
			};
		}

		public void Pickup() {
			Destroy(gameObject);
		}
	}
}
