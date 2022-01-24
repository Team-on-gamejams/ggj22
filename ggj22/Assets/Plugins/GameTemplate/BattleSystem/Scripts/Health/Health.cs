using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BattleSystem.Health {
	public class Health : MonoBehaviour {
		public event Action<HealthCallbackData> onGetDamage;
		public event Action onDie;
		public event Action onReInit;


		public int CurrHealth { get; private set; }
		public int MaxHealth => maxHealth;
		public bool IsDead { get; private set; } = false;
		public Armor Armor => armor;


		[Header("Values")]
		[SerializeField] int maxHealth = 100;
		[Space]
		[SerializeField] Armor armor;
		[Space]
		[SerializeField] [Tooltip("If damage would kill object - left at least [lastChanceHealth]. Often used for player")] bool isHaveLastChance = false;
		[SerializeField] [NaughtyAttributes.ShowIf("isHaveLastChance")] int lastChanceHealth = 5;

		[Header("Refs"), Space]
		[SerializeField] GameObject destroyOnDie;

#if UNITY_EDITOR
		private void Reset() {
			destroyOnDie = gameObject;

			armor = new Armor() {
				type = armor.type,
				baseArmor = armor.baseArmor,
				armorMods = new SerializedDictionary<DamageType, float>() {
					{ DamageType.NormalDamage, 1.0f },
					{ DamageType.HealDamage, 0.0f },
				}
			};
		}
#endif


		private void Start() {
			ReInitHealth(maxHealth, maxHealth);
		}

		public int GetDamage(Damage damageStruct, ArmorType? armorTypeOverride) {
			if (IsDead)
				return 0;

			int damage = 0;
			bool isLastChance = false;
			ArmorType armorType = armorTypeOverride.HasValue ? armorTypeOverride.Value : armor.type;

			switch (damageStruct.type) {
				case DamageType.HealDamage:
					damage = Heal(damageStruct, armorType);
					break;

				default:
					damage = GetActualDamage(damageStruct, out isLastChance, armorType);
					break;
			}

			if (damage == 0)
				return 0;

			onGetDamage?.Invoke(new HealthCallbackData(damageStruct.type, armorType, damage, isLastChance, CurrHealth == 0));

			if (CurrHealth == 0)
				Die();

			return damage;
		}

		int GetActualDamage(Damage damageStruct, out bool isNeedLastChance, ArmorType armorType) {
			int damage = damageStruct.GetDamage(armor, armorType);

			isNeedLastChance = isHaveLastChance && lastChanceHealth < CurrHealth && CurrHealth + damage <= 0;

			if (isNeedLastChance) {
				CurrHealth = lastChanceHealth;
			}
			else {
				CurrHealth = Mathf.Clamp(CurrHealth + damage, 0, MaxHealth);
			}

			return damage;
		}

		int Heal(Damage damageStruct, ArmorType armorType) {
			int heal = damageStruct.GetDamage(armor, armorType);

			if (CurrHealth + heal > MaxHealth)
				heal = MaxHealth - CurrHealth;

			CurrHealth += heal;

			return heal;
		}

		void Die() {
			if (IsDead)
				return;
			IsDead = true;

			onDie?.Invoke();

			if (destroyOnDie)
				Destroy(destroyOnDie);
		}

		void ReInitHealth(int currHp, int maxHp) {
			CurrHealth = currHp;
			maxHealth = maxHp;
			onReInit?.Invoke();
		}

		public struct HealthCallbackData {
			public DamageType damageType;
			public ArmorType armorType;
			public int recievedDamage;
			public bool isLastChance;
			public bool isDie;

			public HealthCallbackData(DamageType damageType, ArmorType armorType, int recievedDamage, bool isLastChance, bool isDie) {
				this.damageType = damageType;
				this.armorType = armorType;
				this.recievedDamage = recievedDamage;
				this.isLastChance = isLastChance;
				this.isDie = isDie;
			}
		}
	}
}
