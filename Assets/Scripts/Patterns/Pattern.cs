using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Pattern : ScriptableObject 
	{
		public virtual Vector3 GetPositionInContainerForIndex (Container container, int index)
		{
			return Vector3.zero;
		}

		public virtual Quaternion GetRotationInContainerForIndex (Container container, int index)
		{
			return Quaternion.identity;
		}
	}
}
