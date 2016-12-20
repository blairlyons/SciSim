using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;

namespace SciSim
{
	public class Agent : MonoBehaviour, IAgent
	{
		public float size;
		public Units units;
		public ExternalBehavior behavior;
		public List<Visualization> visualizationPrefabs = new List<Visualization>();

		Factory _factory;
		public Factory factory
		{
			get 
			{
				if (_factory == null)
				{
					_factory = GetComponentInParent<Factory>();
				}
				return _factory;
			}
		}

		float _currentResolution = 100;
		public float currentResolution
		{
			get 
			{
				return _currentResolution;
			}
			set 
			{
				if (_currentResolution != value)
				{
					_currentResolution = value;
					Visualize();
				}
			}
		}

		public Visualization visualization;

		public void Init () 
		{
			SetScale();
			AddBehavior();
			Visualize();
		}

		void SetScale ()
		{
			transform.localScale = size * ScaleUtility.MultiplierFromMeters( factory.container.units ) / ScaleUtility.MultiplierFromMeters( units ) * Vector3.one;
		}

		void AddBehavior ()
		{
			//TODO
		}

		void Visualize ()
		{
			if (visualization != null)
			{
				Destroy( visualization.gameObject );
			}

			Visualization prefab = visualizationPrefabs.Find( viz => viz.resolution == currentResolution );
			if (prefab != null)
			{
				CreateVisualization( prefab.gameObject );
			}
		}

		void CreateVisualization (GameObject prefab)
		{
			visualization = (Instantiate( prefab, transform.position, transform.rotation ) as GameObject).GetComponent<Visualization>();
			visualization.transform.SetParent( transform );
			visualization.transform.localScale = Vector3.one;
		}

		public virtual bool IsSameAgent (IAgent other)
		{
			return other.GetType() == GetType();
		}
	}
}
