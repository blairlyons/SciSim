using UnityEngine;
using System.Collections;

namespace SciSim
{
	[CreateAssetMenu( fileName = "GaussianDistribution", menuName = "Distributions/Gaussian Distribution", order = 2 )]
	public class GaussianDistribution : Distribution 
	{
		public override float GetConcentrationAtLocalPosition (Vector3 localPosition, float radius, Units units)
		{
			//todo
			return GetAverageConcentration(radius, units);
		}
	}
}
