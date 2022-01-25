using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem {
	public class PowersManager : MonoBehaviour {
		static public PowersManager Instance { get; set; }

		public event Action<Dictionary<PowerPair, float>> onPowersReapply;

		public SerializedDictionary<PowerCondition, Sprite> ConditionSprites => conditionSprites;
		public SerializedDictionary<PowerPair, Sprite> PairsSprites => pairsSprites;
		public SerializedDictionary<PowerCondition, string> ConditionStringKeys => conditionStringKeys;
		public SerializedDictionary<PowerPair, string> PairsStringKeys => pairsStringKeys;

		public PowerPair[] Pairs => allPairs;
		public PowerCondition[] Conditions => allConditions;

		//////////////////////////////////// Values to set from user
		public bool IsMoving {
			get => isMoving;
			set {
				if(value != isMoving) {
					isMoving = value;
					ApplyPowers();
				}
			}
		}
		bool isMoving = false;
		////////////////////////////////////

		//////////////////////////////////// Values to get from user
		Dictionary<PowerPair, float> modifiers = new Dictionary<PowerPair, float>();
		////////////////////////////////////

		[Header("Global refs"), Space]
		[SerializeField] SerializedDictionary<PowerCondition, Sprite> conditionSprites;
		[SerializeField] SerializedDictionary<PowerPair, Sprite> pairsSprites;
		[Space]
		[SerializeField] SerializedDictionary<PowerCondition, string> conditionStringKeys;
		[SerializeField] SerializedDictionary<PowerPair, string> pairsStringKeys;

		List<Power> allPowers = new List<Power>();
		PowerPair[] allPairs;
		PowerCondition[] allConditions;

		private void Awake() {
			Instance = this;

			allPairs = (PowerPair[])Enum.GetValues(typeof(PowerPair));
			foreach (PowerPair pair in allPairs)
				modifiers.Add(pair, 1.0f);;

			allConditions = (PowerCondition[])Enum.GetValues(typeof(PowerCondition));
		}

		public void AddPower(Power power) {
			allPowers.Add(power);

			ApplyPowers();
		}
		
		void ApplyPowers() {
			foreach (PowerPair pair in allPairs)
				modifiers[pair] = 1.0f;

			foreach (var power in allPowers) {
				float mod;
				switch (power.condition) {
					case PowerCondition.Move:
						mod = power.GetMod(IsMoving);
						break;

					case PowerCondition.Stay:
						mod = power.GetMod(!IsMoving);
						break;

					default:
						Debug.LogError("Not inplemented power type");
						mod = 1.0f;
						break;
				}

				modifiers[power.pair] += mod - 1.0f;
			}

			onPowersReapply?.Invoke(modifiers);
		}
	}
}
