using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
	[Serializable]
	public struct Damage {
		public Fraction fraction;
		public DamageType type;
		public int baseDamage;
		[Space]
		public bool isIgnoreArmor;
		public bool isFriendlyFire;
		[Space]
		public SerializedDictionary<ArmorType, float> damageMods;

		public int GetDamage(Armor armor, ArmorType armorType) {
			float totalDamage = baseDamage;

			switch (type) {
				case DamageType.HealDamage:
					break;
				default:
					totalDamage = -totalDamage;
					break;
			}

			if(damageMods != null && damageMods.ContainsKey(armorType))
				totalDamage *= damageMods[armorType];

			if (!isIgnoreArmor) {
				float reduction = armor.GetReductionForDamage(this);

				if(totalDamage < 0)
					totalDamage += reduction;
				else
					totalDamage -= reduction;
			}

			return Mathf.RoundToInt(totalDamage);
		}
	}
}
