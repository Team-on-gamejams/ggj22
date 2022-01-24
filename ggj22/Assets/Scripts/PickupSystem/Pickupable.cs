using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PickupSystem {
	public class Pickupable : MonoBehaviour {
		public static Pickupable Selected;
		public static List<Pickupable> SelectedPickupables = new List<Pickupable>(4);

		[Header("UI"), Space]
		[SerializeField] CanvasGroup descriptioncg;

		[Space]
		[SerializeField] UnityEvent onPickup;

		Vector3 startSize;

		private void Awake() {
			startSize = descriptioncg.transform.localScale;
			descriptioncg.transform.localScale = Vector3.zero;
		}

		private void OnDestroy() {
			SelectedPickupables.Remove(this);
		}

		public void Pickup() {
			onPickup?.Invoke();
		}

		public void OnPlayerEnter(Collider other) {
			if (other.isTrigger || !other.CompareTag("Player"))
				return;

			SelectedPickupables.Add(this);
		}

		public void OnPlayerExit(Collider other) {
			if (other.isTrigger || !other.CompareTag("Player"))
				return;

			SelectedPickupables.Remove(this);
		}

		void Show() {
			LeanTween.cancel(descriptioncg.gameObject);
			LeanTween.scale(descriptioncg.gameObject, startSize, 0.35f)
				.setEase(LeanTweenType.easeOutBack);
		}

		void Hide() {
			LeanTween.cancel(descriptioncg.gameObject);
			LeanTween.scale(descriptioncg.gameObject, Vector3.zero, 0.35f)
				.setEase(LeanTweenType.easeInBack);
		}

		static public void ShowNearest() {
			if (SelectedPickupables.Count == 0) {
				if (Selected)
					Selected.Hide();
				Selected = null;
				return;
			}

			var oldSelected = Selected;
			Selected = SelectedPickupables[0];
			float nearestDist = (PickupAnchor.anchor.position - Selected.transform.position).sqrMagnitude;

			for (int i = 1; i < SelectedPickupables.Count; ++i) {
				float dist = (PickupAnchor.anchor.position - SelectedPickupables[i].transform.position).sqrMagnitude;
				if(dist < nearestDist) {
					nearestDist = dist;
					Selected = SelectedPickupables[i];
				}
			}

			Selected.Show();
			if (oldSelected && oldSelected != Selected)
				oldSelected.Hide();
		}
	}
}
