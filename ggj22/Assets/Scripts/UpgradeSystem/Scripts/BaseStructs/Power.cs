using System;
using System.Text;
using UnityEngine;
using Polyglot;

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

		#region Strings
		public string GetBuffPairString() {
			return GetBuffStringBase(buffPower);

		}

		public string GetDeBuffPairString() {
			return GetBuffStringBase(debuffPower);
		}

		public string GetConditionString() {
			return Localization.Get(PowersManager.Instance.ConditionStringKeys[condition]);
		}

		string GetBuffStringBase(float power) {
			StringBuilder sb = new StringBuilder();

			float value = Mathf.RoundToInt((power - 1.0f) * 100);

			if (value > 0)
				sb.Append('+');
			sb.Append(value);
			sb.Append("% ");
			sb.Append(Localization.Get(PowersManager.Instance.PairsStringKeys[pair]));

			return sb.ToString();
		}
		#endregion
	}
}
