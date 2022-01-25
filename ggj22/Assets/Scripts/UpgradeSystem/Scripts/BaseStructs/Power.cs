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

		public float buffPower;
		public float debuffPower;

		public float GetMod(bool condition) {
			if (condition)
				return buffPower;
			return debuffPower;
		}

		public static Power GetRandomPower() {
			PowerCondition powerCondition = PowersManager.Instance.Conditions.Random();
			PowerPair pair = PowersManager.Instance.Pairs.Random();
			float buffPower = 1.25f;
			float debuffPower = 0.85f;

			switch (powerCondition) {
				case PowerCondition.Stay:
					while(pair == PowerPair.MoveSpeed) {
						pair = PowersManager.Instance.Pairs.Random();
					}
					break;
			}

			switch (pair) {
				case PowerPair.TimeControl:
					buffPower = 0.75f;
					debuffPower = 1.15f;
					break;

				case PowerPair.Armor:
					buffPower = 2f;
					debuffPower = 0.5f;
					break;
			}

			return new Power() {
				condition = powerCondition,
				pair = pair,
				buffPower = buffPower,
				debuffPower = debuffPower,
			};
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
