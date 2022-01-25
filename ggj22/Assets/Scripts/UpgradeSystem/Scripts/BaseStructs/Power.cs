using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem
{
	[Serializable]
    public struct Power
    {
		public PowerCondition condition;
		public PowerPair pair;

		public float buffPower;// = 1.25f;
		public float debuffPower;// = 0.85f;

		public float GetMod(bool condition) {
			if (condition)
				return buffPower;
			return debuffPower;
		}
	}
}
