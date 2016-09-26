using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Agent : MonoBehaviour 
	{
		public float size;
		public Units units;
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
			SetScale();
			Visualize();
		}

		void SetScale ()
		{
			transform.localScale = size * ScaleUtility.MultiplierFromMeters( factory.container.units ) / ScaleUtility.MultiplierFromMeters( units ) * Vector3.one;
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
					CreateVisualization( prefab.gameObject );
					AddAmbientAnimation();
				}
			}
		}

		void CreateVisualization (GameObject prefab)
		{
			visualization = (Instantiate( prefab, transform.position, transform.rotation ) as GameObject).GetComponent<Visualization>();
			visualization.transform.SetParent( transform );
			visualization.transform.localScale = Vector3.one;
		}

		void AddAmbientAnimation ()
		{
			Animator animation = visualization.gameObject.AddComponent<Animator>();
			animation.runtimeAnimatorController = Resources.Load("Animation/Ambient") as RuntimeAnimatorController;
			animation.SetFloat( "randomTimeOffset", Random.Range( 0, 1f ) );
			animation.SetFloat( "sizeOffset", 0.5f / transform.localScale.x );
			animation.SetFloat( "speed", 1f / transform.localScale.x );
		}
	}
}
