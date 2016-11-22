using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Container : MonoBehaviour 
	{
		public float temperature = 310.15f;
		public Bounds bounds = new Bounds( Vector3.zero, 2f * Vector3.one );

		public float _volume = -1f;
		public float volume 
		{
			get 
			{
				if (_volume < 0)
				{
//					_volume = TODO
				}
				return _volume;
			}
		}

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

		void OnDrawGizmos ()
		{
			Gizmos.DrawWireCube( transform.position + bounds.center, 2f * bounds.extents );
		}
	}
}
