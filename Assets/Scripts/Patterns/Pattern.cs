﻿using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Pattern : ScriptableObject 
	{
		public virtual Vector3 GetPositionInContainer (Container container, int index, int n)
		{
			return Vector3.zero;
		}

		public virtual Quaternion GetRotationInContainer (Container container, int index, int n)
		{
			return Quaternion.identity;
		}
	}
}
