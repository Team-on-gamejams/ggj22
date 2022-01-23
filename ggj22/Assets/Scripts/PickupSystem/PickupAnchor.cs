using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PickupSystem
{
    public class PickupAnchor : MonoBehaviour
    {
		public static Transform anchor;

		private void Awake() {
			if (anchor) {
				Debug.LogError("Can have more than 2 pickup anchors");
				Destroy(gameObject);
				return;
			}

			anchor = transform;
		}

		private void Update() {
			Pickupable.ShowNearest();
		}
	}
}
