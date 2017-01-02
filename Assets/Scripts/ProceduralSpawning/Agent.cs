using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Agent : MonoBehaviour 
	{
		public float size;
		public Units units;

		public Distribution childDistribution;
		public Agent childAgent;

		public List<Visualization> visualizationPrefabs = new List<Visualization>();

		float _currentResolution = 100f;
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
			Visualize();
		}

		void SetScale ()
		{
			transform.localScale = size * ScaleUtility.MultiplierFromMeters( BubbleGenerator.Instance.currentUnits ) / ScaleUtility.MultiplierFromMeters( units ) * Vector3.one;
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