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
				return (parentAgent == null ? ZoomController.Instance.currentScale : parentAgent.diameter) 
					* ScaleUtility.ConvertUnitMultiplier((parentAgent == null ? ZoomController.Instance.currentUnits : parentAgent.units), units);
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

		SphereCollider sphereCollider;

		void SetupPhysics ()
		{
			sphereCollider = GetComponent<Collider>() as SphereCollider;
			if (sphereCollider == null)
			{
				sphereCollider = gameObject.AddComponent<SphereCollider>();
			}
			sphereCollider.isTrigger = true;
			sphereCollider.radius = 0.5f;
		}

		void Visualize ()
		{
			if (visualization != null)
			{
				Destroy( visualization.gameObject );
			}
			if (visualizationPrefabs.Count > 0)
			{
				Visualization prefab = visualizationPrefabs[0];//.Find( viz => viz.resolution == currentResolution );
				if (prefab != null)
				{
					CreateVisualization( prefab.gameObject );
				}
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

		float worldScale
		{
			get
			{
				Transform t = transform;
				float scale = transform.localScale.x;
				while (t.parent != null)
				{
					t = t.parent;
					scale *= t.localScale.x;
				}
				return scale;
			}
		}

		void OnDrawGizmos ()
		{
			Gizmos.DrawWireSphere(transform.position, worldScale / 2f);
		}

		public virtual bool IsSameAgent (IAgent other)
		{
			return other.GetType() == GetType();
		}
	}
}