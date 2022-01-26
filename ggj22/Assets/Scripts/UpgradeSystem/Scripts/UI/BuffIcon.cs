using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeSystem.UI {
	public class BuffIcon : MonoBehaviour {
		[Header("Refs"), Space]
		[SerializeField] Image powerImage;

		[SerializeField] Image buffImage;
		[SerializeField] Image debuffImage;

		public void SetSprite(PowerPair pair, bool isBuff) {
			Sprite sprite = PowersManager.Instance.PairsSprites[pair];
			powerImage.sprite = sprite;

			UpdateArrow(isBuff);
		}

		public void UpdateArrow(bool isBuff) {
			buffImage.gameObject.SetActive(isBuff);
			debuffImage.gameObject.SetActive(!isBuff);
		}
	}
}
