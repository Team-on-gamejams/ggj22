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

		public void SetSprite(PowerPair pair, SerializedDictionary<PowerPair, Sprite> sprites, bool isBuff) {
			Sprite sprite = sprites[pair];
			powerImage.sprite = sprite;

			buffImage.gameObject.SetActive(buffImage);
			debuffImage.gameObject.SetActive(!buffImage);
		}
	}
}
