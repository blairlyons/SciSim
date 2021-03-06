﻿using UnityEngine;
using System.Collections;

namespace SciSim
{
	[CreateAssetMenu( fileName = "RandomPattern", menuName = "Patterns/Random Pattern", order = 3 )]
	public class RandomPattern : Pattern 
	{
		public override Vector3 GetPosition (Container container, int index, int n)
		{
			Vector3 center = container.bounds.center;
			Vector3 extents = container.bounds.extents;

			float x = Random.Range( center.x - extents.x, center.x + extents.x );
			float y = Random.Range( center.y - extents.y, center.y + extents.y );
			float z = Random.Range( center.z - extents.z, center.z + extents.z );

			return new Vector3( x, y, z );
		}

		public override Quaternion GetRotation (Container container, int index, int n)
		{
			return Random.rotation;
		}
	}
}
