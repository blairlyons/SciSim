using UnityEngine;
using System.Collections;

namespace SciSim
{
	[CreateAssetMenu( fileName = "RandomCondition", menuName = "Conditions/Random", order = 2 )]
	public class RandomCondition : Condition 
	{
		public float probability = 0.5f;
	}
}
