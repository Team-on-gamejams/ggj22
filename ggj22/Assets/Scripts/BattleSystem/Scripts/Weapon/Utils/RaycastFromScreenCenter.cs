using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Weapons.Utils
{
    public class RaycastFromScreenCenter : MonoBehaviour
    {
		Ray ray;
		Vector2 screenCenter;
		RaycastHit[] hits;
		int len;
		int i;

		float dist;
		float distMin;
		RaycastHit minHit;

		float maxDist = 50;
		int layerMask;

		private void Awake() {
			screenCenter = new Vector2(0.5f, 0.5f);
			hits = new RaycastHit[8];
			layerMask = LayerMask.GetMask("Default");
		}

		private void Update() {
			ray = TemplateGameManager.Instance.Camera.ViewportPointToRay(screenCenter);
			len = Physics.RaycastNonAlloc(ray, hits, maxDist, layerMask, QueryTriggerInteraction.Ignore);

			if (len == 0) {
				transform.position = ray.origin + ray.direction * maxDist;
			}
			else {
				minHit = hits[0];
				distMin = (ray.origin - hits[0].point).sqrMagnitude;

				for (i = 1; i < len; ++i) {
					dist = (ray.origin - hits[i].point).sqrMagnitude;

					if (distMin > dist) {
						distMin = dist;
						minHit = hits[i];
					}
				}

				transform.position = minHit.point;
			}
		}
	}
}
