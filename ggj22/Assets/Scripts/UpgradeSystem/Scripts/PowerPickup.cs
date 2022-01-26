using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityForge;
using UnityForge.PropertyDrawers;
using UpgradeSystem.UI;

namespace UpgradeSystem {
	public class PowerPickup : MonoBehaviour {
		public Power Power => power;

		[Header("Popup"), Space]
		[SerializeField] PowerPopup popup;

		[Header("Power"), Space]
		[SerializeField] bool isRandomPower = true;
		[SerializeField] Power power;

		private void Start() {
			if (isRandomPower) {
				power = Power.GetRandomPower();
			}

			popup.Init(power, true);
		}

		public void Pickup() {
			PowersManager.Instance.AddPower(power);

			LeanTween.value(gameObject, gameObject.transform.localPosition.y, gameObject.transform.localPosition.y - 12, 1.0f)
				.setEase(LeanTweenType.easeInQuad)
				.setOnUpdate((float y) => {
					gameObject.transform.localPosition = gameObject.transform.localPosition.SetY(y);
				})
				.setOnComplete(()=> {
					Destroy(gameObject, 0.5f);
				});
		}
	}
}
