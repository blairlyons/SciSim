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
		public List<ConditionNode> conditions = new List<ConditionNode>();
		public List<EffectNode> effects = new List<EffectNode>();
	}

	[System.Serializable]
	public class ConditionNode
	{
		public ConditionType type;
		public Condition condition;
	}

	[System.Serializable]
	public class EffectNode
	{
		public EffectType type;
		public Effect effect;
	}

	[System.Serializable]
	public class Condition : ScriptableObject
	{
		
	}

	[System.Serializable]
	public class Effect : ScriptableObject
	{
		
	}

	public enum ConditionType
	{
		Random,
		CollideWithTarget,
		SiteOccupancy,
		TargetCanFormProduct
	}

	public enum EffectType
	{
		MoveTarget,
		ToggleSiteOccupancy,
		ReactTarget,
		ReleaseTarget
	}
}
