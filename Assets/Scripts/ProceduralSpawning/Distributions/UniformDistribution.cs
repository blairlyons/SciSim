using UnityEngine;
using System.Collections;

namespace SciSim
{
	[CreateAssetMenu( fileName = "UniformDistribution", menuName = "Distributions/Uniform Distribution", order = 1 )]
	public class UniformDistribution : Distribution 
	{
		public override float GetConcentrationAtLocalPosition (Vector3 localPosition, float radius, Units units)
		{
			return GetAverageConcentration(radius, units);
		}
	}
}
