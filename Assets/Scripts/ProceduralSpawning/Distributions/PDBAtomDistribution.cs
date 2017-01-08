using UnityEngine;
using System.Collections;

namespace SciSim
{
	[CreateAssetMenu( fileName = "PDBAtomDistribution", menuName = "Distributions/PDB Atom Distribution", order = 3 )]
	public class PDBAtomDistribution : Distribution 
	{
		public override float GetConcentrationAtLocalPosition (Vector3 localPosition, float radius, Units units)
		{
			//todo
			return maxConcentration;
		}

		public override Vector3 GetPosition (Vector3 bubblePosition, float bubbleRadius, Vector3 agentPosition, float agentRadius, int index, int n)
		{
			return Vector3.zero;
		}
	}
}
