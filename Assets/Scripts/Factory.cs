using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Factory : MonoBehaviour 
	{
		public int count;
		public List<Visualization> visualizationPrefabs = new List<Visualization>();
		public List<Rule> rules = new List<Rule>();

		Container _container;
		public Container container
		{
			get 
			{
				if (_container == null)
				{
					_container = GetComponentInParent<Container>();
				}
				return _container;
			}
		}

		public Agent[] agents;

		void Start () 
		{
			MakeAgents();
		}

		void Update () 
		{
			foreach (Agent agent in agents)
			{
				if (agent != null)
				{
					agent.Tick();
				}
			}
		}

		public void MakeAgents ()
		{
			agents = new Agent[count];
			for (int i = 0; i < count; i++)
			{
				agents[i] = new GameObject(name + "_" + i, typeof(Agent)).GetComponent<Agent>();
				agents[i].transform.parent = transform;
				agents[i].transform.position = container.RandomPointInBounds();
				agents[i].transform.rotation = Random.rotation;
				agents[i].Init();
			}
		}
	}
}
