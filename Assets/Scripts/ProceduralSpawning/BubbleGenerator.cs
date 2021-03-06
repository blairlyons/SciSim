﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class BubbleGenerator : MonoBehaviour 
	{
		public float spawnRadius = 10f;
		public Units currentUnits = Units.Nanometers;
		public LayerMask agentLayer;
		public List<Agent> overlappingAgents = new List<Agent>();

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
			c.radius = spawnRadius / ZoomController.Instance.currentScale;

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
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere( transform.position, spawnRadius );
		}
	}
}