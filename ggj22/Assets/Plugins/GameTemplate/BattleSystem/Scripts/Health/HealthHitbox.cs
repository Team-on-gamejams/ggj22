using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BattleSystem.Health {
	public class HealthHitbox : MonoBehaviour {
		public ArmorType ArmorType => isOverrideArmorType ? armorTypeOverride : health.Armor.type;

		[Header("Refs"), Space]
		[SerializeField] Health health;
		[SerializeField] bool isOverrideArmorType;
		[SerializeField] [NaughtyAttributes.ShowIf("isOverrideArmorType")] ArmorType armorTypeOverride = ArmorType.WeakSpotArmor;

		public void GetDamage(Damage damage) {
			health.GetDamage(damage, isOverrideArmorType ? armorTypeOverride : null);
		}

		public bool IsSameParent(HealthHitbox hitbox) {
			return hitbox.health == health;
		}
	}
}
