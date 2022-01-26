using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeSystem.UI {
	public class ConditionIcon : MonoBehaviour {
		[Header("Refs"), Space]
		[SerializeField] Image conditionImage;

		public void SetSprite(PowerCondition condition) {
			Sprite sprite = PowersManager.Instance.ConditionSprites[condition];
			conditionImage.sprite = sprite;
		}
	}
}
