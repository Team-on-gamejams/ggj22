using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
	[Serializable]
	public struct Armor {
		public ArmorType type;
		public int baseArmor;
		[Space]
		public SerializedDictionary<DamageType, float> armorMods;

		public int GetDamage(Damage damage) {
			return damage.GetDamage(this);
		}

		public float GetReductionForDamage(Damage damage) {
			float reduction = baseArmor;

			if (armorMods != null && armorMods.ContainsKey(damage.type))
				reduction *= armorMods[damage.type];

			return reduction;
		}
	}
}
