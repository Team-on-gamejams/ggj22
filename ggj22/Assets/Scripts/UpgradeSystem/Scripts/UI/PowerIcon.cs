using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem.UI {
	public class PowerIcon : MonoBehaviour {
		[Header("Refs"), Space]
		[SerializeField] ConditionIcon conditionIcon;
		[SerializeField] BuffIcon buffIcon;

		Power power;

		public void Init(Power _power) {
			power = _power;

			buffIcon.SetSprite(power.pair, true);
			conditionIcon.SetSprite(power.condition);
		}

		public void UpdateArrow() {
			buffIcon.UpdateArrow(power.GetConditionBool());
		}
	}
}
