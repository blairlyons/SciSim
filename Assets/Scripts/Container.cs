using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Container : MonoBehaviour 
	{
		public float temperature = 310.15f;
		public Bounds bounds = new Bounds( Vector3.zero, 2f * Vector3.one );
		public Units units;

		Factory[] _factories;
		public Factory[] factories
		{
			get 
			{
				if (_factories == null)
				{
					_factories = GetComponentsInChildren<Factory>();
				}
				return _factories;
			}
		}

		public Vector3 RandomPointInBounds ()
		{
			float x = Random.Range( bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x );
			float y = Random.Range( bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y );
			float z = Random.Range( bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z );

			return transform.position + new Vector3( x, y, z );
		}

		void OnDrawGizmos ()
		{
			Gizmos.DrawWireCube( transform.position + bounds.center, 2f * bounds.extents );
		}
	}
}
