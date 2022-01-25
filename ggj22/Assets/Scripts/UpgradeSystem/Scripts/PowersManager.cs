using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem {
	public class PowersManager : MonoBehaviour {
		static public PowersManager Instance { get; set; }

		public event Action<Dictionary<PowerPair, float>> onPowersReapply;

		//Values to set from user
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

		//Values to get from user
		Dictionary<PowerPair, float> modifiers = new Dictionary<PowerPair, float>();

		List<Power> allPowers = new List<Power>();
		PowerPair[] allPairs;

		private void Awake() {
			Instance = this;

			allPairs = (PowerPair[])Enum.GetValues(typeof(PowerPair));
			foreach (PowerPair pair in allPairs)
				modifiers.Add(pair, 1.0f);;
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

				modifiers[power.pair] *= mod;
			}

			onPowersReapply?.Invoke(modifiers);
		}
	}
}
