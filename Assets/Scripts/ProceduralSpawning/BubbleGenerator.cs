using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class BubbleGenerator : MonoBehaviour 
	{
		public delegate void ScaleChange();
		public static event ScaleChange OnScaleChange;

		public float currentScale = 1f; // one unit in the unity editor scene view = 1 nm (for example)
		public Units currentUnits = Units.Nanometers;
		public float spawnRadius = 10f;
		public LayerMask agentLayer;
		public List<Agent> overlappingAgents = new List<Agent>();

		Agent _anchorAgent;
		public Agent anchorAgent
		{
			get
			{
				if (_anchorAgent == null)
				{
					UpdateAnchorAgent();
				}
				return _anchorAgent;
			}
		}

		void UpdateAnchorAgent ()
		{
			Transform agent = GameObject.FindObjectOfType<Agent>().transform;
			while (agent.parent != null && agent.parent.GetComponent<Agent>())
			{
				agent = agent.parent;
			}
			_anchorAgent = agent.GetComponent<Agent>();
		}

		static BubbleGenerator _Instance;
		public static BubbleGenerator Instance
		{
			get 
			{
				if (_Instance == null)
				{
					_Instance = GameObject.FindObjectOfType<BubbleGenerator>();
				}
				return _Instance;
			}
		}

		public void Zoom (float dZoom)
		{
			currentScale += dZoom * currentScale;
			CheckChangeUnits();

			if (OnScaleChange != null)
			{
				OnScaleChange();
			}
		}

		void CheckChangeUnits ()
		{
			if (currentScale < 1f || currentScale > 1E3f)
			{
				currentScale = Mathf.Clamp(currentScale, 1f, 1E3f);
				Units newUnits = ScaleUtility.GetNextScale(currentUnits, currentScale > 1f ? 1 : -1);
				currentScale = currentScale * ScaleUtility.ConvertUnitMultiplier(currentUnits, newUnits);
				currentUnits = newUnits;
			}
		}

		void Start () 
		{
			SetupPhysics();
		}

		void SetupPhysics ()
		{
			SphereCollider c = GetComponent<Collider>() as SphereCollider;
			if (c == null)
			{
				c = gameObject.AddComponent<SphereCollider>();
			}
			c.isTrigger = true;
			c.radius = currentScale;

			Rigidbody r = GetComponent<Rigidbody>();
			if (r == null)
			{
				r = gameObject.AddComponent<Rigidbody>();
			}
			r.useGravity = false;

//			gameObject.layer = agentLayer;
		}

		void OnTriggerEnter (Collider other)
		{
			Agent otherAgent = other.GetComponent<Agent>();
			if (otherAgent != null && !overlappingAgents.Contains(otherAgent))
			{
				AddOverlappingAgent(otherAgent);
			}
		}

		void AddOverlappingAgent (Agent overlappingAgent)
		{
			overlappingAgents.Add(overlappingAgent);
			//todo subscribe listeners
		}

		void OnTriggerExit (Collider other)
		{
			Agent otherAgent = other.GetComponent<Agent>();
			if (otherAgent != null && overlappingAgents.Contains(otherAgent))
			{
				RemoveOverlappingAgent(otherAgent);
			}
		}

		void RemoveOverlappingAgent (Agent overlappingAgent)
		{
			overlappingAgents.Remove(overlappingAgent);
			//todo unsubscribe listeners
		}

		bool spawnedBubble;

		void Update () 
		{
			if (!spawnedBubble && overlappingAgents.Count > 0)
			{
				SpawnBubble();
			}
		}

		void SpawnBubble ()
		{
			foreach (Agent agent in overlappingAgents)
			{
				if (agent.childAgent != null)
				{
					int n = NumberOfAgentsForConcentration(agent.GetConcentrationAtPosition(transform.position));
					for (int i = 0; i < n; i++)
					{
						Vector3 position = agent.GetChildPosition(transform.position, spawnRadius, i, n);
						Quaternion rotation = agent.GetChildRotation(i, n);

						Agent newChildAgent = Instantiate(agent.childAgent, position, rotation) as Agent;
						newChildAgent.name = agent.childAgent.name + i;
						newChildAgent.transform.SetParent(agent.transform);
						newChildAgent.Init();
					}
				}
			}
			spawnedBubble = true;
		}

		//move to Distribution
		float avogadro = 6.022E23f; // agents/mol
		int NumberOfAgentsForConcentration (float concentration)
		{
			float volume = 4f/3f * Mathf.PI * Mathf.Pow(spawnRadius * ScaleUtility.ConvertUnitMultiplier(currentUnits, Units.Centimeters), 3f) * 1E-3f; //liters
			return Mathf.RoundToInt(concentration * volume * avogadro);
		}

		void OnDrawGizmos ()
		{
			Gizmos.DrawSphere( transform.position, spawnRadius );
		}
	}
}