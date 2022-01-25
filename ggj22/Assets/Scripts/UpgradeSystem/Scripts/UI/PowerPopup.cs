using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeSystem.UI;
using TMPro;

namespace UpgradeSystem
{
    public class PowerPopup : MonoBehaviour
    {
		[Header("Popup"), Space]
		[SerializeField] BuffIcon buffIcon;
		[SerializeField] TextMeshProUGUI buffTextField;
		[Space]
		[SerializeField] ConditionIcon condionIcon;
		[SerializeField] TextMeshProUGUI conditionTextField;
		[Space]
		[SerializeField] TextMeshProUGUI otherwiseTextField;
		[Space]
		[SerializeField] BuffIcon debuffIcon;
		[SerializeField] TextMeshProUGUI debuffTextField;
		[Space]
		[SerializeField] GameObject pickupTip;

		public void Init(Power power, bool isCanBePickedup) {
			buffIcon.SetSprite(power.pair, true);
			buffTextField.text = power.GetBuffPairString();

			condionIcon.SetSprite(power.condition);
			conditionTextField.text = power.GetConditionString();

			debuffIcon.SetSprite(power.pair, false);
			debuffTextField.text = power.GetDeBuffPairString();

			pickupTip.gameObject.SetActive(isCanBePickedup);
		}
	}
}
