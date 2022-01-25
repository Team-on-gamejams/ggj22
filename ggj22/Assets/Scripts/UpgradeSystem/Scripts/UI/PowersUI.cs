using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem.UI {
	public class PowersUI : MonoBehaviour {
		[Header("Prefabs"), Space]
		[SerializeField] GameObject powerIconPrefab;

		List<PowerIcon> icons = new List<PowerIcon>();

		private void Start() {
			PowersManager.Instance.onPowerAdded += OnPowerAdded;
			PowersManager.Instance.onPowersReapply += OnPowersReapply;
		}

		private void OnDestroy() {
			PowersManager.Instance.onPowerAdded -= OnPowerAdded;
			PowersManager.Instance.onPowersReapply -= OnPowersReapply;
		}

		void OnPowerAdded(Power power) {
			GameObject iconGo = Instantiate(powerIconPrefab, transform);
			PowerIcon icon = iconGo.GetComponent<PowerIcon>();

			icon.Init(power);

			icons.Add(icon);
		}

		void OnPowersReapply(Dictionary<PowerPair, float> powers) {
			foreach (var icon in icons)
				icon.UpdateArrow();
		}
	}
}
