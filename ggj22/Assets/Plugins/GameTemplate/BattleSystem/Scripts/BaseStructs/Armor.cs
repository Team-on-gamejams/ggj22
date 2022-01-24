using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
	[Serializable]
	public struct Armor {
		public Fraction fraction;
		public ArmorType type;
		public int baseArmor;
		[Space]
		public SerializedDictionary<DamageType, float> armorMods;

		public float GetReductionForDamage(Damage damage) {
			float reduction = baseArmor;

			if (armorMods != null && armorMods.ContainsKey(damage.type))
				reduction *= armorMods[damage.type];

			return reduction;
		}
	}
}
