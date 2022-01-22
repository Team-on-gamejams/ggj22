using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
	public enum DamageType : byte {
		NormalDamage,
		HealDamage,
	}

	public static class DamageTypeExtensions {
		public static Color ToColor(this DamageType type) {
			switch (type) {
				case DamageType.NormalDamage:
					return Color.grey;
				case DamageType.HealDamage:
					return Color.green;
				default:
					return Color.white;
			}
		}
	}
}
