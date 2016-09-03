using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	[CreateAssetMenu( fileName = "Rule", menuName = "Rule", order = 1 )]
	public class Rule : ScriptableObject 
	{
		public List<RuleNode> nodes = new List<RuleNode>();

		public void Setup (Agent agent)
		{
			foreach (RuleNode node in nodes)
			{
				node.Setup( agent );
			}
		}
	}

	[System.Serializable]
	public class RuleNode
	{
		public List<Condition> conditions = new List<Condition>();
		public List<Effect> effects = new List<Effect>();

		public void Setup (Agent agent)
		{
			foreach (Condition condition in conditions)
			{
				condition.Setup( agent );
			}
			foreach (Effect effect in effects)
			{
				effect.Setup( agent );
			}
		}
	}

	public delegate void RuleDelegate ();

	[System.Serializable]
	public class Condition : ScriptableObject
	{
		protected Agent agent;

		public virtual void Setup (Agent _agent)
		{
			agent = _agent;
		}

		public virtual void Test ()
		{

		}

		public void Satisfied ()
		{
			Debug.Log( agent.name + " " + GetType().ToString() + " is satisfied" );
		}
	}

	[System.Serializable]
	public class Effect : ScriptableObject
	{
		protected Agent agent;

		public virtual void Setup (Agent _agent)
		{
			agent = _agent;
		}

		public virtual void Execute ()
		{

		}

		public void ExecutionComplete ()
		{
			Debug.Log( agent.name + " " + GetType().ToString() + " execution complete" );
		}
	}
}
