using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Agent : MonoBehaviour 
	{
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

		bool VisualizationIsIncorrect
		{
			get 
			{
				return (visualization == null) || (visualization.resolution != currentResolution);
			}
		}

		public Visualization visualization;

		public void Init () 
		{
			Visualize();
			AddBehaviors();
		}

		void Visualize ()
		{
			if (VisualizationIsIncorrect)
			{
				if (visualization != null)
				{
					Destroy( visualization.gameObject );
				}

				Visualization prefab = factory.visualizationPrefabs.Find( viz => viz.resolution == currentResolution );
				if (prefab != null)
				{
					visualization = (Instantiate( prefab.gameObject, transform.position, transform.rotation ) as GameObject).GetComponent<Visualization>();
					visualization.transform.parent = transform;
				}
			}
		}

		void AddBehaviors ()
		{
			foreach (Rule rule in factory.rules)
			{
				//TODO
			}
		}

		public void Tick () 
		{
			//TODO
		}
	}
}
