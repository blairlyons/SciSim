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
			if (agentPrefab != null && pattern != null)
			{
				int n = Random.Range( count - variation, count + variation );

				Agent agent;
				agents = new List<Agent>();
				for (int i = 0; i < n; i++)
				{
					agent = Instantiate( agentPrefab ).GetComponent<Agent>();
					agent.transform.parent = transform;
					agent.transform.position = GetPositionForIndex( i, n );
					agent.transform.rotation = GetRotationForIndex( i, n );

					agents.Add( agent );
					agent.Init();
				}
			}
		}

		Vector3 GetPositionForIndex (int index, int n)
		{
			return container.transform.position + pattern.GetPositionInContainer( container, index, n );
		}

		Quaternion GetRotationForIndex (int index, int n)
		{
			return container.transform.rotation * pattern.GetRotationInContainer( container, index, n );
		}
	}
}
