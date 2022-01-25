using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeSystem.UI {
	public class ConditionIcon : MonoBehaviour {
		[Header("Refs"), Space]
		[SerializeField] Image conditionImage;

		public void SetSprite(PowerCondition condition, SerializedDictionary<PowerCondition, Sprite> sprites) {
			Sprite sprite = sprites[condition];
			conditionImage.sprite = sprite;
		}
	}
}
