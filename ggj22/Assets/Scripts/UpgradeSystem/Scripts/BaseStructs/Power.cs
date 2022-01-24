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
	}
}
