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
		public Agent agentPrefab;

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

		public void MakeAgents ()
		{
			if (agentPrefab != null)
			{
				int n = Random.Range( count - variation, count + variation );

				Agent agent;
				agents = new List<Agent>();
				for (int i = 0; i < n; i++)
				{
					agent = Instantiate( agentPrefab ).GetComponent<Agent>();
					agent.transform.parent = transform;
					agent.transform.position = GetPositionForIndex( i );
					agent.transform.rotation = GetRotationForIndex( i );

					agents.Add( agent );
					agent.Init();
				}
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
