using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Agent : MonoBehaviour 
	{
		public bool initOnStart;
		public float radius;
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

		void Start ()
		{
			if (initOnStart)
			{
				Init();
			}
		}

		public void Init () 
		{
			SetupPhysics();
			SetScale();
			Visualize();
		}

		void SetupPhysics ()
		{
			SphereCollider c = GetComponent<Collider>() as SphereCollider;
			if (c == null)
			{
				c = gameObject.AddComponent<SphereCollider>();
			}
			c.isTrigger = true;
			c.radius = 0.5f;
		}

		void SetScale ()
		{
			transform.localScale = 2f * radius * ScaleUtility.MultiplierFromMeters( BubbleGenerator.Instance.currentUnits ) / ScaleUtility.MultiplierFromMeters( units ) * Vector3.one;
		}

		void Visualize ()
		{
			if (visualization != null)
			{
				Destroy( visualization.gameObject );
			}
			Debug.Log(visualizationPrefabs[0].resolution + " == " + currentResolution);
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

		public float GetConcentrationAtPosition (Vector3 position)
		{
			return childDistribution.GetConcentrationAtLocalPosition(position - transform.position, radius, units);
		}

		public Vector3 GetChildPosition (Vector3 bubblePosition, float bubbleRadius, int index, int n)
		{
			return bubblePosition + childDistribution.GetPosition(bubblePosition, bubbleRadius, transform.position, radius, index, n);
		}

		public Quaternion GetChildRotation (int index, int n)
		{
			return childDistribution.GetRotation(index, n);
		}

		void OnDrawGizmos ()
		{
			Gizmos.DrawWireSphere(transform.position, radius);
		}

		public virtual bool IsSameAgent (IAgent other)
		{
			return other.GetType() == GetType();
		}
	}
}