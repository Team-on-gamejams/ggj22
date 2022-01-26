using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityForge.PropertyDrawers;
using BattleSystem.Weapons.UI;

namespace BattleSystem.Weapons {
	abstract public class BaseWeapon : MonoBehaviour {
		public enum WeaponState : byte { 
			Ready,			// Ready to attack
			Starting,       // Can be interrupted
			Performing,     // Single frame with actual attack
			Ending,         // Can't be interrupted
			Ended,          // Single frame between ending and cooldown
			Cooldown,       // can't be used
		}

		public event Action onStartAttack;
		public Action onHitAttack;
		public Action onMissAttack;
		public event Action onEndAttack;
		public event Action<float, float> onCooldownUpdate;

		public float AttackSpeedModifier {
			get => attackSpeedModifier;
			set {
				if(attackSpeedModifier != value) {
					attackSpeedModifier = value;
					if(animator)
						animator.speed = attackSpeedModifier;
				}
			}
		}
		float attackSpeedModifier = 1.0f;

		public WeaponUIData UIData => uiData;
		public float CooldownTime => cooldownTime / AttackSpeedModifier;
		public float StartingTime => startingTime / AttackSpeedModifier;
		public float EndingTime => endingTime / AttackSpeedModifier;


		[Header("Values"), Space]
		[SerializeField] protected Damage damage;

		[Header("Visuals"), Space]
		[SerializeField] protected WeaponUIData uiData;

		[Header("Animation"), Space]
		[SerializeField] Animator animator;
		[SerializeField] [NaughtyAttributes.ShowIf("IsAnimator")] string emptyAnimationName = "Empty";
		[SerializeField] [NaughtyAttributes.ShowIf("IsAnimator")] string attackTriggerName;
		[SerializeField] [NaughtyAttributes.ShowIf("IsAnimator")] int layer = 1;

		[Header("Timings"), Space]
		[SerializeField] float cooldownTime;
		[SerializeField] [NaughtyAttributes.ShowIf("IsNoAnimator")] float startingTime;
		[SerializeField] [NaughtyAttributes.ShowIf("IsNoAnimator")] float endingTime;

		bool isPlayerHoldInput;
		bool isDoSingleAttack;

		float timer;

		WeaponState state;

#if UNITY_EDITOR
		private void Reset() {
			animator = GetComponent<Animator>();

			damage = new Damage() {
				type = damage.type,
				baseDamage = damage.baseDamage, 

				isIgnoreArmor = damage.isIgnoreArmor,

				damageMods = new SerializedDictionary<ArmorType, float> {
					{ ArmorType.NormalArmor, 1.0f },
					{ ArmorType.WeakSpotArmor, 1.5f },
					{ ArmorType.ArmoredArmor, 0.5f },
				}
			};
		}
#endif

		void Update() {
			if (state == WeaponState.Ready) {
				if (isDoSingleAttack || isPlayerHoldInput) {
					state = WeaponState.Starting;

					if (IsAnimator())
						animator.SetTrigger(attackTriggerName);
					StartAttack();
					onStartAttack?.Invoke();
				}
			}

			if(state == WeaponState.Starting) {
				if (!IsAnimator()) {
					if(timer >= StartingTime) {
						timer -= StartingTime;
						state = WeaponState.Performing;
					}
					else {
						timer += Time.deltaTime;
					}
				}
			}

			if(state == WeaponState.Performing) {
				DoAttack();
				state = WeaponState.Ending;
				onCooldownUpdate?.Invoke(0, CooldownTime);
			}

			if (state == WeaponState.Ending) {
				if (!IsAnimator()) {
					if (timer >= EndingTime) {
						timer -= EndingTime;
						state = WeaponState.Ended;
					}
					else {
						timer += Time.deltaTime;
					}
				}
			}

			if (state == WeaponState.Ended) {
				isDoSingleAttack = false;
				onEndAttack?.Invoke();
				state = WeaponState.Cooldown;
			}

			if (state == WeaponState.Cooldown) {
				if (timer >= CooldownTime) {
					timer -= CooldownTime;
					state = WeaponState.Ready;
					onCooldownUpdate?.Invoke(CooldownTime, CooldownTime);
				}
				else {
					timer += Time.deltaTime;
					onCooldownUpdate?.Invoke(timer, CooldownTime);
				}
			}
		}

		public bool IsAttacking() {
			return isDoSingleAttack || isPlayerHoldInput || (state != WeaponState.Ready && state != WeaponState.Cooldown);
		}

		public bool IsCanInterruptAttack() {
			return state != WeaponState.Performing && state != WeaponState.Ending && state != WeaponState.Ended;
		}

		public void InterruptAttack() {
			isDoSingleAttack = false;
			
			if (state != WeaponState.Cooldown) {
				state = WeaponState.Ready;
				timer = 0;

				if (animator)
					animator.Play(emptyAnimationName, layer);
			}
		}

		#region Attack
		virtual protected void StartAttack() { }
		abstract protected void DoAttack();
		#endregion

		#region Scripts interface
		public bool IsCanAttack() {
			return state == WeaponState.Ready;
		}

		public void DoSingleAttack() {
			timer = 0;
			isDoSingleAttack = true;
		}
		#endregion

		#region Player Interface
		public void OnInputActionDown() {
			isPlayerHoldInput = true;
		}

		public void OnInputActionUp() {
			isPlayerHoldInput = false;
		}
		#endregion

		#region AnimationCallbacks
		bool IsAnimator() => animator != null;
		bool IsNoAnimator() => !IsAnimator();

		public void AnimationCallbackPerformAttack() {
			if(state == WeaponState.Starting)
				state = WeaponState.Performing;
		}

		public void AnimationCallbackEndAttack() {
			if(state == WeaponState.Ending)
				state = WeaponState.Ended;
		}
		#endregion

		#region Powers
		virtual public void ApplyAttackSpeedMod(float mod) {
			AttackSpeedModifier = mod;
		}

		virtual public void ApplyDamageMod(float mod) {
			damage.Modifier = mod;
		}
		#endregion
	}
}
