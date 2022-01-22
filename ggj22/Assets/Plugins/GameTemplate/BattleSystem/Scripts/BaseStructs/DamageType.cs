using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
	public enum DamageType : byte {
		NormalDamage,
		HealDamage,
	}

	public static class DamageTypeExtensions {
		public static Color ToColor(this DamageType type, ArmorType armorType) {
			switch (type) {
				case DamageType.NormalDamage:
					switch (armorType) {
						case ArmorType.NormalArmor:
							return Color.gray;

						case ArmorType.WeakSpotArmor:
							return Color.yellow;

						case ArmorType.ArmoredArmor:
							return Color.gray;

						default:
							return Color.gray;
					}

				case DamageType.HealDamage:
					return Color.green;

				default:
					return Color.white;
			}
		}
	}
}
