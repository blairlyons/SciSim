using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Factory : MonoBehaviour 
	{
		public int count;
		public int variation;

		public Pattern pattern;
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

		public List<Agent> agents;

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
			int n = Random.Range( count - variation, count + variation );

			agents = new List<Agent>();
			for (int i = 0; i < n; i++)
			{
				agents.Add( new GameObject(name + "_" + i, typeof(Agent)).GetComponent<Agent>() );
				agents[i].transform.parent = transform;
				agents[i].transform.position = GetPositionForIndex( i );
				agents[i].transform.rotation = GetRotationForIndex( i );
				agents[i].Init();
			}
		}

		Vector3 GetPositionForIndex (int index)
		{
			return container.transform.position + pattern.GetPositionInContainerForIndex( container, index );
		}

		Quaternion GetRotationForIndex (int index)
		{
			return container.transform.rotation * pattern.GetRotationInContainerForIndex( container, index );
		}
	}
}
