using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
	public enum DamageType : byte {
		None,
		Heal,
	}

	public static class DamageTypeExtensions {
		public static Color ToColor(this DamageType type) {
			switch (type) {
				case DamageType.None:
					return Color.grey;
				case DamageType.Heal:
					return Color.green;
				default:
					return Color.white;
			}
		}
	}
}
