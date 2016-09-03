using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	[CreateAssetMenu( fileName = "Rule", menuName = "Rule", order = 1 )]
	public class Rule : ScriptableObject 
	{
		public List<RuleNode> nodes = new List<RuleNode>();
	}

	[System.Serializable]
	public class RuleNode
	{
		public List<Condition> conditions = new List<Condition>();
		public List<Effect> effects = new List<Effect>();
	}

	[System.Serializable]
	public class Condition : ScriptableObject
	{
		
	}

	[System.Serializable]
	public class Effect : ScriptableObject
	{
		
	}
}
