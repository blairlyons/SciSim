﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class BubbleGenerator : MonoBehaviour 
	{
		public delegate void ScaleChange();
		public static event ScaleChange OnScaleChange;

		public float currentRadius = 10f;
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

		public void Zoom (float dZoom)
		{
			currentRadius += Mathf.Min(50f, (dZoom / Mathf.Abs(dZoom)) * Mathf.Pow(10f, Mathf.Log10(currentRadius) + dZoom - 1f));
			CheckChangeUnits();
		}

		void CheckChangeUnits ()
		{
			if (currentRadius < 1f || currentRadius > 1E3f)
			{
				currentRadius = Mathf.Clamp(currentRadius, 1f, 1E3f);
				Units newUnits = ScaleUtility.GetNextScale(currentUnits, currentRadius > 1f ? 1 : -1);
				currentRadius = currentRadius * ScaleUtility.ConvertUnits(currentUnits, newUnits);
				currentUnits = newUnits;

				if (OnScaleChange != null)
				{
					OnScaleChange();
				}
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
			c.radius = currentRadius;

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
						Vector3 position = agent.GetChildPosition(transform.position, currentRadius, i, n);
						Quaternion rotation = agent.GetChildRotation(i, n);

						Agent newChildAgent = Instantiate(agent.childAgent, position, rotation) as Agent;
						newChildAgent.transform.SetParent(agent.transform);
						newChildAgent.Init();
					}
				}
			}
			spawnedBubble = true;
		}

		float avogadro = 6.022E23f; // agents/mol

		int NumberOfAgentsForConcentration (float concentration)
		{
			float volume = 4f/3f * Mathf.PI * Mathf.Pow(currentRadius * ScaleUtility.ConvertUnits(currentUnits, Units.Centimeters), 3f) * 1E-3f; //liters
			return Mathf.RoundToInt(concentration * volume * avogadro);
		}

		void OnDrawGizmos ()
		{
			Gizmos.DrawSphere( transform.position, currentRadius );
		}
	}
}