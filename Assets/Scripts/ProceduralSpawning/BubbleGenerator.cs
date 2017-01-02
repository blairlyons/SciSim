using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class BubbleGenerator : MonoBehaviour 
	{
		public Units currentUnits;
		public LayerMask agentLayer;
		List<Agent> cloudAgents = new List<Agent>();

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
			
		}

		void CreateCollider ()
		{

		}

		void OnTriggerEnter (Collider other)
		{

		}

		void OnTriggerExit (Collider other)
		{

		}

		void Update () 
		{
			
		}
	}
}