using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Factory : MonoBehaviour, IFactory
	{
		public int count;
		public int variation;

		public Pattern pattern;
		public GeneratorAgent agentPrefab;

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

		public List<GeneratorAgent> agents;

		void Start () 
		{
			MakeAgents();
		}

		public void MakeAgents ()
		{
			if (agentPrefab != null && pattern != null)
			{
				int n = Random.Range( count - variation, count + variation );

				GeneratorAgent agent;
				agents = new List<GeneratorAgent>();
				for (int i = 0; i < n; i++)
				{
					agent = Instantiate( agentPrefab ).GetComponent<GeneratorAgent>();
					agent.transform.SetParent( transform );
					agent.transform.position = GetPosition( i, n );
					agent.transform.rotation = GetRotation( i, n );

					agents.Add( agent );
					agent.Init();
				}
			}
		}

		Vector3 GetPosition (int index, int n)
		{
			return container.transform.position + pattern.GetPosition( container, index, n );
		}

		Quaternion GetRotation (int index, int n)
		{
			return container.transform.rotation * pattern.GetRotation( container, index, n );
		}
	}
}
