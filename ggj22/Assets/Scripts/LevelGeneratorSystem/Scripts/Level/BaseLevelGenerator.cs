using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneratorSystem.Level {
	public class BaseLevelGenerator : MonoBehaviour {
		void Start() {
			GenerateRoomSequence();
		}

		void GenerateRoomSequence() {
			GenerateLevel();
			OnEndGenerateLevel();
		}

		protected virtual void GenerateLevel() { }

		protected virtual void OnEndGenerateLevel() { }
	}
}
