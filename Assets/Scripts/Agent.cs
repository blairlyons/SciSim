using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Agent : MonoBehaviour 
	{
		public List<Visualization> visualizationPrefabs = new List<Visualization>();
		public Container container;
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
		}

		void Visualize ()
		{
			if (VisualizationIsIncorrect)
			{
				if (visualization != null)
				{
					Destroy( visualization.gameObject );
				}

				Visualization prefab = visualizationPrefabs.Find( viz => viz.resolution == currentResolution );
				if (prefab != null)
				{
					visualization = (Instantiate( prefab.gameObject, transform.position, transform.rotation ) as GameObject).GetComponent<Visualization>();
					visualization.transform.parent = transform;
				}
			}
		}
	}
}
