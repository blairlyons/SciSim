using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Agent : MonoBehaviour 
	{
		public bool active;
		public bool initOnStart;
		public float radius;
		public Units units;

		public Distribution childDistribution;
		public Agent childAgent;

		public List<Visualization> visualizationPrefabs = new List<Visualization>();

		public float diameter 
		{
			get
			{
				return 2f * radius;
			}
		}

		public float parentScale // in this agent's units
		{ 
			get
			{
				return (parentAgent == null ? BubbleGenerator.Instance.currentScale : parentAgent.diameter) 
					* ScaleUtility.ConvertUnitMultiplier((parentAgent == null ? BubbleGenerator.Instance.currentUnits : parentAgent.units), units);
			}
		}

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

		Agent _parentAgent;
		public Agent parentAgent
		{
			get 
			{
				if (_parentAgent == null && transform.parent != null)
				{
					_parentAgent = transform.parent.GetComponent<Agent>();
				}
				return _parentAgent;
			}
		}

		void Start ()
		{
			if (initOnStart)
			{
				Init();
			}
		}

		public void Init () 
		{
			UpdateScale();
			SetupPhysics();
			Visualize();
		}

		public void UpdateScale ()
		{ 
			transform.localScale = diameter / parentScale * Vector3.one;
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
			Gizmos.DrawWireSphere(transform.position, 
				radius * ScaleUtility.ConvertUnitMultiplier(units, (parentAgent == null ? BubbleGenerator.Instance.currentUnits : parentAgent.units)));
		}

		public virtual bool IsSameAgent (IAgent other)
		{
			return other.GetType() == GetType();
		}
	}
}